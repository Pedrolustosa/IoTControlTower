﻿using IoTControlTower.Domain.Interface.UserRepository;

namespace IoTControlTower.Domain.Validation;

public interface IUnitOfWork
{
    IDevicesRepository DevicesRepository { get; }
    IUserRepository UserRepository { get; }

    Task CommitAsync();
}