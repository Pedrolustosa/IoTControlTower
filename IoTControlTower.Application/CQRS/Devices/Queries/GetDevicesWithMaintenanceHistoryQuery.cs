using MediatR;
using IoTControlTower.Domain.Entities;

namespace IoTControlTower.Application.CQRS.Devices.Queries;

public class GetDevicesWithMaintenanceHistoryQuery : IRequest<IEnumerable<Device>> { }
