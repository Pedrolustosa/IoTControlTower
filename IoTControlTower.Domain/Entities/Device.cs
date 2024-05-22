namespace IoTControlTower.Domain.Entities
{
    public class Device
    {
        public int Id { get; private set; }
        public string Description { get; private set; }
        public string Manufacturer { get; private set; }
        public string Url { get; private set; }
        public bool IsActive { get; private set; }

        public string UserId { get; private set; }
        public User User { get; private set; }

        public ICollection<CommandDescription> CommandDescriptions { get; private set; }

        protected Device() { }

        public Device(string description, string manufacturer, string url, bool isActive, string userId)
        {
            SetDescription(description);
            SetManufacturer(manufacturer);
            SetUrl(url);
            SetIsActive(isActive);
            SetUser(userId);
        }

        public void SetDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Description is required");
            Description = description;
        }

        public void SetManufacturer(string manufacturer)
        {
            Manufacturer = manufacturer;
        }

        public void SetUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentException("URL is required");
            Url = url;
        }

        public void SetIsActive(bool isActive)
        {
            IsActive = isActive;
        }

        public void SetUser(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentException("UserId is required");
            UserId = userId;
        }
    }
}
