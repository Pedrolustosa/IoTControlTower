namespace IoTControlTower.Application.DTO
{
    public class DashboardSummaryDTO
    {
        public int TotalDevices { get; set; }
        public int ActiveDevices { get; set; }
        public int InactiveDevices { get; set; }
        public int TotalUsers { get; set; }
        public int MonitoringDevices { get; set; }
    }

}
