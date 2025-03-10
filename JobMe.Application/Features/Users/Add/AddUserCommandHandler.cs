using Application.Responses;
using Application.Handlers;
using Domain.Interfaces.Repositories;
using Domain.Entities;

namespace Features.Users.Add
{
    public class AddUserCommandHandler : QuickRequestHandler<AddUserCommandRequest>
    {
        IGenericRepository<User> _userRepository;
        public AddUserCommandHandler(IGenericRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public override async Task<BaseResponse> Handle(AddUserCommandRequest request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                return new BaseResponse
                {
                    IsSuccessful = false,
                    Message = "Invalid Request",
                    Data = request.Notifications
                };
            }

            var newUSer = new User {
                Username = request.DataModel.Username,
                Password = request.DataModel.Password,
                Email = request.DataModel.Email
            };

            await _userRepository.AddAsync(newUSer);
            
            return new BaseResponse
            {
                IsSuccessful = true,
                Message = "User added successfully",
                Data = newUSer
            };
        }
    }
}