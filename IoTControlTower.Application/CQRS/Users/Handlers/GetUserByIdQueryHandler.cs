﻿using MediatR;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Application.CQRS.Users.Queries;

namespace IoTControlTower.Application.CQRS.Users.Handlers;

public class GetUserByIdQueryHandler(IUserDapperRepository userDapperRepository) : IRequestHandler<GetUserByIdQuery, User>
{
    private readonly IUserDapperRepository _userDapperRepository = userDapperRepository;

    public async Task<User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userDapperRepository.GetDeviceById(request.Id);
            return user;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
