namespace IoTControlTower.Application.DTO.Users
{
    public class UserUpdateDTO : UserDTO
    {
        public DateTime? UpdateDate { get; set; } = DateTime.Now;
    }
}
