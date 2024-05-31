using MediatR;
using IoTControlTower.Domain.Entities;

namespace IoTControlTower.Application.CQRS.Devices.Queries;

public class GetDevicesQuery : IRequest<IEnumerable<Device>> { }
