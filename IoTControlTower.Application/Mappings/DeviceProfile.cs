using AutoMapper;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Application.DTO.Device;
using IoTControlTower.Application.CQRS.Devices.Commands;

namespace IoTControlTower.Application.Mapping;

public class DeviceProfile : Profile
{
    public DeviceProfile()
    {
        try
        {
            CreateMap<Device, DeviceDTO>().ReverseMap();
            CreateMap<DeviceDTO, CreateDeviceCommand>().ReverseMap();
            CreateMap<DeviceDTO, UpdateDeviceCommand>().ReverseMap();
        }
        catch (AutoMapperConfigurationException ex)
        {
            Console.WriteLine($"An AutoMapperConfigurationException occurred: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while configuring mapping profiles: {ex.Message}");
            throw;
        }
    }
}
