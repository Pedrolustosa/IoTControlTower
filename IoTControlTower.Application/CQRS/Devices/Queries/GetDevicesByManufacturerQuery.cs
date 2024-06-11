using MediatR;
using IoTControlTower.Domain.Entities;

namespace IoTControlTower.Application.CQRS.Devices.Queries;

public class GetDevicesByManufacturerQuery : IRequest<IEnumerable<Device>>
{
    public string Manufacturer { get; set; }
}
