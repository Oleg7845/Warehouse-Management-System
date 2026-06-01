using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Formatting.Json;
using System.IO;
using System.Windows;
using WarehouseManagementSystem.Application.Abstractions.Cryptography;
using WarehouseManagementSystem.Application.Abstractions.Factories.AuditLogs;
using WarehouseManagementSystem.Application.Abstractions.Factories.Users;
using WarehouseManagementSystem.Application.Abstractions.Persistence.Repositories;
using WarehouseManagementSystem.Application.Abstractions.Security;
using WarehouseManagementSystem.Application.Abstractions.Services;
using WarehouseManagementSystem.Application.Abstractions.Utilities;
using WarehouseManagementSystem.Application.Factories.AuditLogs;
using WarehouseManagementSystem.Application.Factories.Users;
using WarehouseManagementSystem.Application.Services;
using WarehouseManagementSystem.Infrastructure.Cryptography.Hashing;
using WarehouseManagementSystem.Infrastructure.Persistence.Abstractions;
using WarehouseManagementSystem.Infrastructure.Persistence.Database;
using WarehouseManagementSystem.Infrastructure.Persistence.Repositories;
using WarehouseManagementSystem.Infrastructure.Security;
using WarehouseManagementSystem.Infrastructure.Utilities;
using WarehouseManagementSystem.UI.Navigation;
using WarehouseManagementSystem.UI.Pages.Dashboard;
using WarehouseManagementSystem.UI.Pages.Authentication.ForcePasswordChange;
using WarehouseManagementSystem.UI.Pages.Authentication.InitialAdminSetup;
using WarehouseManagementSystem.UI.Pages.Authentication.Login;
using WarehouseManagementSystem.UI.Pages.Main;
using WarehouseManagementSystem.UI.Pages.Settings;
using WarehouseManagementSystem.UI.Pages.Shell;
using WarehouseManagementSystem.UI.Pages.SupportTickets.SupportTicketCreate;
using WarehouseManagementSystem.UI.Pages.SupportTickets.SupportTicketView;
using WarehouseManagementSystem.UI.Pages.SupportCenter;
using WarehouseManagementSystem.UI.Pages.SupportPanel;
using WarehouseManagementSystem.UI.Pages.Users.UserCreate;
using WarehouseManagementSystem.UI.Pages.Users.UserList;
using WarehouseManagementSystem.UI.Pages.Users.UserView;
using WarehouseManagementSystem.UI.Services;
using WarehouseManagementSystem.UI.Abstractions;
using WarehouseManagementSystem.UI.Pages.AuditLogs.AuditLogView;
using WarehouseManagementSystem.UI.Pages.AuditLogs.AuditLogList;
using WarehouseManagementSystem.Application.Factories.SupportTickets;
using WarehouseManagementSystem.UI.Pages.SupportTickets.SupportTicketList;
using WarehouseManagementSystem.Domain.Exceptions;
using WarehouseManagementSystem.Domain.Enums;
using WarehouseManagementSystem.UI.Pages.Products.ProductCreate;
using WarehouseManagementSystem.UI.Pages.Products.ProductList;
using WarehouseManagementSystem.UI.Pages.Products.ProductView;
using WarehouseManagementSystem.UI.Pages.Categories.CategoryView;
using WarehouseManagementSystem.UI.Pages.StockOperations.StockOperationView;
using WarehouseManagementSystem.UI.Pages.Suppliers.SupplierView;
using WarehouseManagementSystem.UI.Pages.Categories.CategoryCreate;
using WarehouseManagementSystem.UI.Pages.Categories.CategoryList;
using WarehouseManagementSystem.UI.Pages.StockOperations.StockOperationCreate;
using WarehouseManagementSystem.UI.Pages.StockOperations.StockOperationList;
using WarehouseManagementSystem.UI.Pages.Suppliers.SupplierCreate;
using WarehouseManagementSystem.UI.Pages.Suppliers.SupplierList;

namespace WarehouseManagementSystem.UI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : System.Windows.Application
{
    /// <summary>
    /// Global service provider used for dependency resolution.
    /// </summary>
    public static IServiceProvider? Services { get; private set; }

    /// <summary>
    /// Shortcut to the main ViewModel.
    /// </summary>
    public MainViewModel? MainViewModel => Services?.GetService<MainViewModel>();

    protected override void OnStartup(StartupEventArgs e)
    {
        InitLogger();
        RegisterGlobalExceptionHandlers();

        Services = ConfigureServices();

        var mainView = Services.GetRequiredService<MainView>();
        MainWindow = mainView;

        var navigation = Services.GetRequiredService<INavigationService>();
        var notificationService = Services.GetRequiredService<INotificationService>();
        

        Task.Run(async () =>
        {
            try
            {
                var factory = Services.GetRequiredService<IDbContextFactory>();
                factory.Initialize();

                using var scope = Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
                var initializer = scope.ServiceProvider.GetRequiredService<IAppInitializer>();
                bool hasUsers = await initializer.HasAnyUsersAsync();

                Current.Dispatcher.Invoke(() =>
                {
                    mainView.Show();

                    if (hasUsers)
                    {
                        navigation.NavigateTo<LoginViewModel>();
                    }
                    else
                    {
                        navigation.NavigateTo<InitialAdminSetupViewModel>();
                    }
                });
            }
            catch (DomainException ex)
            {
                Current.Dispatcher.Invoke(() =>
                {
                    mainView.Show();

                    navigation.NavigateTo<LoginViewModel>();

                    notificationService.ShowNotification(
                        text: ex.UserMessage,
                        NotifyType.Error);
                });
            }
            catch (Exception ex)
            {
                Current.Dispatcher.Invoke(() =>
                {
                    mainView.Show();

                    navigation.NavigateTo<LoginViewModel>();

                    notificationService.ShowNotification(
                        text: "Application error",
                        NotifyType.Error);
                });
            }
        });

        base.OnStartup(e);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        Log.CloseAndFlush();
        base.OnExit(e);
    }

    /// <summary>
    /// Registers application services and builds the dependency container.
    /// </summary>
    private IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        // Core application service
        services.AddSingleton<IAppService, AppService>();

        // Database factory
        services.AddSingleton<IDbContextFactory>(serviceProvider =>
        {
            var appService = serviceProvider.GetRequiredService<IAppService>();
            return new DbContextFactory(appService.DbConnectionString);
        });

        // Repositories
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IAuditLogRepository, AuditLogRepository>();
        services.AddTransient<ISupportTicketRepository, SupportTicketRepository>();
        services.AddTransient<IProductRepository, ProductRepository>();

        // Factories
        services.AddTransient<IUserFactory, UserFactory>();
        services.AddTransient<IUserAuditLogFactory, UserAuditLogFactory>();
        services.AddTransient<ISupportTicketAuditLogFactory, SupportTicketAuditLogFactory>();
        services.AddTransient<ISupportTicketFactory, SupportTicketFactory>();

        // Domain services
        services.AddScoped<IAppInitializer, AppInitializer>();
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<INotificationService, NotificationService>();
        services.AddSingleton<IUserSessionService, UserSessionService>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IShellNavigationService, ShellNavigationService>();
        services.AddTransient<IPasswordHashingService, Argon2PasswordHasher>();
        services.AddTransient<IPasswordGenerator, PasswordGenerator>();
        services.AddTransient<ISkuGenerator, SkuGenerator>();
        services.AddTransient<IAuditLogService, AuditLogService>();
        services.AddTransient<ISupportTicketService, SupportTicketService>();
        services.AddTransient<IUserService, UserService>();

        // ViewModels
        services.AddSingleton<MainViewModel>();
        services.AddTransient<InitialAdminSetupViewModel>();
        services.AddTransient<ForcePasswordChangeViewModel>();
        services.AddTransient<LoginViewModel>();
        services.AddTransient<ShellViewModel>();
        services.AddTransient<DashboardViewModel>();

        services.AddTransient<ProductCreateViewModel>();
        services.AddTransient<ProductListViewModel>();
        services.AddTransient<ProductViewModel>();

        services.AddTransient<UserCreateViewModel>();
        services.AddTransient<UserListViewModel>();
        services.AddTransient<UserViewModel>();

        services.AddTransient<AuditLogListViewModel>();
        services.AddTransient<AuditLogViewModel>();

        services.AddTransient<CategoryCreateViewModel>();
        services.AddTransient<CategoryListViewModel>();
        services.AddTransient<CategoryViewModel>();

        services.AddTransient<StockOperationCreateViewModel>();
        services.AddTransient<StockOperationListViewModel>();
        services.AddTransient<StockOperationViewModel>();

        services.AddTransient<SupplierCreateViewModel>();
        services.AddTransient<SupplierListViewModel>();
        services.AddTransient<SupplierViewModel>();

        services.AddTransient<SupportTicketListViewModel>();
        services.AddTransient<SupportTicketCreateViewModel>();
        services.AddTransient<SupportTicketViewModel>();
        services.AddTransient<SupportCenterViewModel>();
        services.AddTransient<SupportPanelViewModel>();

        services.AddTransient<SettingsViewModel>();

        // Views
        services.AddSingleton<MainView>();

        return services.BuildServiceProvider();
    }

    /// <summary>
    /// Registers handlers for unhandled exceptions across the application.
    /// </summary>
    private void RegisterGlobalExceptionHandlers()
    {
        AppDomain.CurrentDomain.UnhandledException += (s, ex) =>
        {
            var exception = ex.ExceptionObject as Exception;
            Log.Error(exception, "Unhandled AppDomain exception");

            if (exception != null)
            {
                Current.Dispatcher.Invoke(() => HandleAndShowException(exception));
            }
        };

        DispatcherUnhandledException += (s, ex) =>
        {
            Log.Error(ex.Exception, "Unhandled UI exception");

            HandleAndShowException(ex.Exception);
            ex.Handled = true;
        };

        TaskScheduler.UnobservedTaskException += (s, ex) =>
        {
            Log.Error(ex.Exception, "Unobserved task exception");

            if (ex.Exception != null)
            {
                Current.Dispatcher.Invoke(() => HandleAndShowException(ex.Exception));
            }

            ex.SetObserved();
        };
    }

    /// <summary>
    /// Analyzes the exception and displays a user-friendly notification.
    /// </summary>
    private void HandleAndShowException(Exception exception)
    {
        if (exception is AggregateException aggregateEx)
        {
            exception = aggregateEx.Flatten().InnerException ?? exception;
        }

        string userMessage = "An unexpected application error occurred";

        if (exception is DomainException domainEx)
        {
            userMessage = domainEx.UserMessage;
        }else if (exception is Npgsql.NpgsqlException ||
            exception is InvalidOperationException && exception.InnerException is Npgsql.NpgsqlException ||
            HasInnerException<Npgsql.NpgsqlException>(exception) ||
            HasInnerException<System.Net.Sockets.SocketException>(exception))
        {
            userMessage = "Database connection lost";
        }
        else if (exception is Microsoft.EntityFrameworkCore.DbUpdateException)
        {
            userMessage = "Failed to update database records";
        }

        try
        {
            var notificationService = Services?.GetService<INotificationService>();

            if (notificationService != null)
            {
                notificationService.ShowNotification(userMessage, NotifyType.Error);
            }
            else
            {
                MessageBox.Show(userMessage, "Critical System Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception logEx)
        {
            Log.Fatal(logEx, "Failed to render the global UI error notification layout");
        }
    }

    private bool HasInnerException<T>(Exception? ex) where T : Exception
    {
        if (ex == null) return false;
        if (ex is T) return true;
        return HasInnerException<T>(ex.InnerException);
    }

    /// <summary>
    /// Configures Serilog logging pipeline.
    /// </summary>
    private void InitLogger()
    {
        var logsDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Warehouse Management System",
            "Logs");

        Directory.CreateDirectory(logsDirectory);

        var txtLogPath = Path.Combine(logsDirectory, "log-.txt");
        var jsonLogPath = Path.Combine(logsDirectory, "log-.json");

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Async(a => a.File(
                txtLogPath,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7,
                outputTemplate:
                "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}\n"
            ))
            .WriteTo.Async(a => a.File(
                new JsonFormatter(renderMessage: true),
                jsonLogPath,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7
            ))
            .Enrich.WithProperty("App", "Warehouse Management System")
            .Enrich.WithProperty("Version", "1.0.0")
            .CreateLogger();
    }
}
