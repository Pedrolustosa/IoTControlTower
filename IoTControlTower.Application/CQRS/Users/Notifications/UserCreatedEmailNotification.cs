using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using IoTControlTower.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Routing;
using IoTControlTower.Application.DTO.Email;
using IoTControlTower.Application.Interface;

namespace IoTControlTower.Application.CQRS.Users.Notifications;

public class UserCreatedEmailNotification(ILogger<UserCreatedEmailNotification> logger,
                                          UserManager<User> userManager,
                                          IEmailService emailService,
                                          IUrlHelper iUrlHelper) : INotificationHandler<UserCreatedNotification>
{
    private readonly IUrlHelper _iUrlHelper = iUrlHelper;
    private readonly IEmailService _emailService = emailService;
    private readonly UserManager<User> _userManager = userManager;
    private readonly ILogger<UserCreatedEmailNotification> _logger = logger;

    public async Task Handle(UserCreatedNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handle() - Handling UserCreatedNotification for user: {UserId}", notification.User.Id);
        var urlHelper = _iUrlHelper;
        var schema = _iUrlHelper.ActionContext.HttpContext.Request.Scheme;
        _logger.LogInformation("Handle() - Generating email confirmation token for user: {UserId}", notification.User.Id);
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(notification.User);
        _logger.LogInformation("Handle() - Token generated for user: {UserId}", notification.User.Id);

        string confirmationLink = ConfirmationLink(notification.User, urlHelper, schema, token);
        _logger.LogInformation("Handle() - Confirmation link generated for user: {UserId}", notification.User.Id);

        var message = new MessageDTO(new string[] { notification.User.Email! }, "Confirmation email link", confirmationLink!);
        _logger.LogInformation("Handle() - Sending confirmation email to user: {UserId}", notification.User.Id);
        _emailService.SendEmail(message);
        _logger.LogInformation("Handle() - Confirmation email sent to user: {UserId}", notification.User.Id);
    }

    private string ConfirmationLink(User newUser, IUrlHelper urlHelper, string scheme, string token)
    {
        _logger.LogInformation("ConfirmationLink() - Generating confirmation link for user with email: {Email}", newUser.Email);

        var confirmationLink = urlHelper.Action(new UrlActionContext
        {
            Action = nameof(ConfirmEmail),
            Controller = "Users",
            Values = new { token, email = newUser.Email },
            Protocol = scheme
        });

        if (confirmationLink is null)
        {
            _logger.LogError("ConfirmationLink() - Failed to generate confirmation link.");
            throw new Exception("Failed to generate confirmation link.");
        }

        _logger.LogInformation("ConfirmationLink() - Confirmation link generated successfully for user with email: {Email}", newUser.Email);
        return confirmationLink;
    }

    public async Task<string> ConfirmEmail(string token, string email)
    {
        _logger.LogInformation("ConfirmEmail() - Confirming email for user with email: {Email}", email);

        try
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is not null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    _logger.LogInformation("ConfirmEmail() - Email confirmed successfully for user with email: {Email}", email);
                    return "Success: Email verified successfully";
                }
            }
            _logger.LogWarning("ConfirmEmail() - User with email {Email} does not exist", email);
            return "Error: This user does not exist.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ConfirmEmail() - An error occurred while confirming email for user with email: {Email}", email);
            throw;
        }
    }
}
