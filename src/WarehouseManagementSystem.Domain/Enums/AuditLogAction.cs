namespace WarehouseManagementSystem.Domain.Enums;

public enum AuditLogAction
{
    // User actions
    InitialAdminCreated,

    AdminCreated,
    UserCreated,
    UserActivated,
    UserDeactivated,
    UserLocked,
    UserUnlocked,

    UserLoggedIn,
    UserLoggedOut,
    UserLoginFailed,

    PasswordSet,
    PasswordChanged,
    PasswordReset,

    UserLoginAttemptsReset,


    // SupportTicket actions
    SupportTicketOpened,
    SupportTicketDescriptionUpdated,
    SupportTicketMovedToInProgress,
    SupportTicketWaitingForUser,
    SupportTicketResolved,
    SupportTicketClosed,
}
