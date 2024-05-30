using MediatR;
using IoTControlTower.Domain.Entities;

namespace IoTControlTower.Application.Users.Queries;

public class GetUserByIdQuery : IRequest<User>
{
    public Guid Id { get; set; }

    public class GetUserByIdQueryHandler(IUserDapperRepository userDapperRepository) : IRequestHandler<GetUserByIdQuery, User>
    {
        private readonly IUserDapperRepository _userDapperRepository = userDapperRepository;

        public async Task<User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userDapperRepository.GetDeviceById(request.Id);
                return user;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
