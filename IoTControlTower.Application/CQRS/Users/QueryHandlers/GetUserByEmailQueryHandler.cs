using MediatR;
using Microsoft.Extensions.Logging;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Application.CQRS.Users.Queries;

namespace IoTControlTower.Application.CQRS.Users.QueryHandlers;

public class GetUserByEmailQueryHandler(IUserDapperRepository userDapperRepository,
                                        ILogger<GetUserByEmailQueryHandler> logger) : IRequestHandler<GetUserByEmailQuery, User>
{
    private readonly ILogger<GetUserByEmailQueryHandler> _logger = logger;
    private readonly IUserDapperRepository _userDapperRepository = userDapperRepository;

    public async Task<User> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handle() - Attempting to retrieve user by email: {Email}", request.Email);
        try
        {
            var user = await _userDapperRepository.GetUserByEmail(request.Email);
            if (user is null)
                _logger.LogWarning("Handle() - User not found with email: {Email}", request.Email);
            else
                _logger.LogInformation("Handle() - User retrieved successfully with email: {Email}", request.Email);
            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Handle() - Error occurred while retrieving user by email: {Email}", request.Email);
            throw;
        }
    }
}