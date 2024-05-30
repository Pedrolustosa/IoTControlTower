namespace IoTControlTower.Application.Users.Commands;

public class UpdateUserCommand : UserCommandBase
{
    public Guid Id { get; set; }

    //public class UpdateUserCommandHandler(UnitOfWork unitOfWork, ILogger<UserEFRepository> logger) : IRequestHandler<UpdateUserCommand, User>
    //{
    //    private readonly UnitOfWork _unitOfWork = unitOfWork;
    //    private readonly ILogger<UserEFRepository> _logger = logger;

    //    public async Task<User> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    //    {
    //        try
    //        {
    //            _logger.LogInformation("UpdateDeviceCommand", request.Id);
    //            var existingDevice = await _unitOfWork.UserRepository.GetUserId() ?? throw new InvalidOperationException("Device not found");
    //            existingDevice.Update(request.Description, request.Manufacturer, request.Url, request.IsActive);
    //            _unitOfWork.DevicesRepository.UpdateDeviceAsync(existingDevice);
    //            await _unitOfWork.CommitAsync();
    //            return existingDevice;
    //        }
    //        catch (Exception)
    //        {
    //            throw;
    //        }
    //    }
    //}
}
