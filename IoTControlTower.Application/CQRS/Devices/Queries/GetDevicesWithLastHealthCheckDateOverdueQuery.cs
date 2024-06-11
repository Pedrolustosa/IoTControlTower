using MediatR;
using IoTControlTower.Domain.Entities;

namespace IoTControlTower.Application.CQRS.Devices.Queries;

public class GetDevicesWithLastHealthCheckDateOverdueQuery : IRequest<IEnumerable<Device>> { }
