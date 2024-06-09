﻿using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Infra.Data.Context;
using IoTControlTower.Domain.Interface.UserRepository;

namespace IoTControlTower.Infra.Data.Repositories.UserRepository;

public class UserEFRepository(IoTControlTowerContext context,
                              ILogger<UserEFRepository> logger) : IUserRepository
{
    private readonly IoTControlTowerContext _context = context;
    private readonly ILogger<UserEFRepository> _logger = logger;

    public async Task<User> GetUserByEmail(string email)
    {
        try
        {
            _logger.LogInformation("GetUserByEmail() - Inicialize");
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError($"GetUserByEmail() - Error: {ex.Message}");
            throw;
        }
    }
}