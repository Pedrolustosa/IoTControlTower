﻿namespace IoTControlTower.Application.DTO.Device
{
    public class DeviceCreateDTO
    {
        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public string Url { get; set; }
        public bool IsActive { get; set; }
    }
}