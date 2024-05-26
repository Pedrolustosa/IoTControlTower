namespace IoTControlTower.Application.DTO.User
{
    public class UserUpdateDTO : UserDTO
    {
        public DateTime? UpdateDate { get; set; } = DateTime.Now;
    }
}
