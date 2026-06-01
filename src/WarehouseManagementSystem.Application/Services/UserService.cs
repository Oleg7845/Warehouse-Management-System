using WarehouseManagementSystem.Domain.Exceptions;
using WarehouseManagementSystem.Application.Abstractions.Cryptography;
using WarehouseManagementSystem.Application.Abstractions.Factories.AuditLogs;
using WarehouseManagementSystem.Application.Abstractions.Factories.Users;
using WarehouseManagementSystem.Application.Abstractions.Persistence.Repositories;
using WarehouseManagementSystem.Application.Abstractions.Services;
using WarehouseManagementSystem.Domain.Enums;
using WarehouseManagementSystem.Domain.Models;
using WarehouseManagementSystem.Domain.Queries;
using System.Windows;

namespace WarehouseManagementSystem.Application.Services;

public class UserService : IUserService
{
    private readonly IAppService _appService;
    private readonly IAuditLogService _auditLogService;
    private readonly IUserAuditLogFactory _userAuditLogFactory;
    private readonly IUserSessionService _sessionService;
    private readonly IPasswordHashingService _passwordHashingService;
    private readonly IUserRepository _repository;
    private readonly IUserFactory _factory;

    public UserService(
        IAppService appService,
        IAuditLogService auditLogService,
        IUserAuditLogFactory userAuditLogFactory,
        IPasswordHashingService passwordHashingService,
        IUserSessionService sessionService,
        IUserRepository repository,
        IUserFactory factory)
    {
        _appService = appService;
        _auditLogService = auditLogService;
        _userAuditLogFactory = userAuditLogFactory;
        _sessionService = sessionService;
        _passwordHashingService = passwordHashingService;
        _repository = repository;
        _factory = factory;
    }
    public async Task<User> CreateUserAsync(
        string username, string password,
        UserRole role,
        UserCreationContext context = UserCreationContext.Default)
    {
        if (await _repository.ExistsByUsernameAsync(username))
            throw new UserAlreadyExistsException(username);

        string passwordHash = _passwordHashingService.HashPassword(password);

        User user = role == UserRole.Admin
        ? _factory.CreateAdmin(username, passwordHash)
        : _factory.CreateUser(username, passwordHash);

        User newUser = await _repository.AddAsync(user);

        if (context == UserCreationContext.InitialAdminSetup)
        {
            await WriteSystemLog(
                subject: newUser,
                action: AuditLogAction.InitialAdminCreated);
        }
        else
        {
            AuditLogAction action = role == UserRole.Admin
                ? AuditLogAction.AdminCreated
                : AuditLogAction.UserCreated;

            await WriteAdminLog(
                subject: newUser,
                action: action);
        }

        return newUser;
    }

    public async Task<int> GetUsersCountAsync()
    {
        return await _repository.GetUsersCountAsync();
    }

    public async Task<User> GetUserByUsernameAsync(string username)
    {
        User user = await _repository.GetByUsernameAsync(username);

        return user;
    }

    public async Task<List<User>> GetUsersAsync(UserSearchQuery query)
    {
        return await _repository.GetUsersAsync(query);
    }

    public async Task<int> GetUsersCountAsync(UserSearchQuery query)
    {
        return await _repository.GetUsersCountAsync(query);
    }

    public async Task<User> GetVerifiedUserAsync(string username, string password)
    {
        User user = await _repository.GetByUsernameAsync(username);

        if (user.FailedLoginAttempts == _appService.MaxFailedLoginAttemptsCount)
        {
            user.Lock();
            await _repository.UpdateAsync(user);

            await WriteSystemLog(
                subject: user,
                action: AuditLogAction.UserLocked);

            throw new AccountLockedException();
        }

        if (!_passwordHashingService.VerifyPassword(password, user.PasswordHash))
        {
            user.IncrementFailedLoginAttempts();
            await _repository.UpdateAsync(user);

            await WriteSystemLog(
                subject: user,
                action: AuditLogAction.UserLoginFailed);

            throw new InvalidPasswordException();
        }

        return user;
    }

    public async Task SetUserSession(User user)
    {
        try
        {
            user.UpdateLastLoginTime();
            await _repository.UpdateAsync(user);
            _sessionService.SetUser(user);

            await WriteUserLog(
                actor: user,
                action: AuditLogAction.UserLoggedIn);
        }
        catch (Exception ex)
        {

        }
    }

    public async Task<User> ForceChangePasswordWithLoginAsync(string username, string newPassword)
    {
        User user = await _repository.GetByUsernameAsync(username);

        string passwordHash = _passwordHashingService.HashPassword(newPassword);

        user.ChangePassword(passwordHash);
        user.UpdateLastLoginTime();

        await _repository.UpdateAsync(user);

        await WriteUserLog(
            actor: user,
            action: AuditLogAction.PasswordSet);

        return user;
    }

    public async Task LogoutAsync()
    {
        User currentUser = _sessionService.CurrentUser!;

        _sessionService.ClearUser();

        await WriteUserLog(
            actor: currentUser,
            action: AuditLogAction.UserLoggedOut);
    }

    public async Task ActivateUserAsync(string username)
    {
        User user = await _repository.GetByUsernameAsync(username);

        user.Activate();

        await _repository.UpdateAsync(user);

        await WriteAdminLog(
            subject: user,
            action: AuditLogAction.UserActivated);
    }

    public async Task DeactivateUserAsync(string username)
    {
        User user = await _repository.GetByUsernameAsync(username);

        user.Deactivate();

        await _repository.UpdateAsync(user);

        await WriteAdminLog(
            subject: user,
            action: AuditLogAction.UserDeactivated);
    }

    public async Task LockUserAsync(string username)
    {
        User user = await _repository.GetByUsernameAsync(username);

        user.Lock();

        await _repository.UpdateAsync(user);

        await WriteAdminLog(
            subject: user,
            action: AuditLogAction.UserLocked);
    }

    public async Task UnlockUserAsync(string username)
    {
        User user = await _repository.GetByUsernameAsync(username);

        user.Unlock();

        await _repository.UpdateAsync(user);

        await WriteAdminLog(
            subject: user,
            action: AuditLogAction.UserUnlocked);
    }

    public async Task ResetFailedLoginAttemptsAsync(string username)
    {
        User user = await _repository.GetByUsernameAsync(username);

        user.ResetFailedLoginAttempts();

        await _repository.UpdateAsync(user);

        await WriteAdminLog(
            subject: user,
            action: AuditLogAction.UserLoginAttemptsReset);
    }

    public async Task ChangePasswordAsync(string username, string currentPassword, string newPassword)
    {
        User user = await _repository.GetByUsernameAsync(username);

        if (!_passwordHashingService.VerifyPassword(currentPassword, user.PasswordHash))
            throw new InvalidPasswordException();

        string passwordHash = _passwordHashingService.HashPassword(newPassword);

        user.ChangePassword(passwordHash);

        await _repository.UpdateAsync(user);

        await WriteUserLog(
            actor: user,
            action: AuditLogAction.PasswordChanged);
    }

    public async Task ResetPasswordAsync(string username, string tempPassword)
    {
        User user = await _repository.GetByUsernameAsync(username);

        string passwordHash = _passwordHashingService.HashPassword(tempPassword);

        user.ResetPassword(passwordHash);

        await _repository.UpdateAsync(user);

        await WriteAdminLog(
            subject: user,
            action: AuditLogAction.PasswordReset);
    }

    public string FormatUserCredentials(string username, string password)
    {
        return "System Access Credentials:\n" +
            "Username: " + username + "\n" +
            "Temporary Password: " + password + "\n\n" +
            "Please change your password upon your first login.";
    }

    private async Task WriteSystemLog(User subject, AuditLogAction action)
    {
        AuditLog newlog = _userAuditLogFactory.BuildSystemLog(subject, action);
        await _auditLogService.WriteAsync(newlog);
    }

    private async Task WriteAdminLog(User subject, AuditLogAction action)
    {
        AuditLog newlog = _userAuditLogFactory.BuildAdminLog(
            actor: _sessionService.CurrentUser!,
            subject: subject,
            action: action);

        await _auditLogService.WriteAsync(newlog);
    }

    private async Task WriteUserLog(User actor, AuditLogAction action)
    {
        AuditLog newlog = _userAuditLogFactory.BuildUserLog(actor, action);
        await _auditLogService.WriteAsync(newlog);
    }
}
