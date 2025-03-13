using Application.Responses;
using Application.Handlers;
using Domain.Interfaces.Repositories;
using Domain.Entities;

namespace Features.Users.Update
{
    public class UpdateUserCommandHandler : QuickRequestHandler<UpdateUserCommandRequest>
    {
        IGenericRepository<User> _userRepository;
        public UpdateUserCommandHandler(IGenericRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public override async Task<BaseResponse> Handle(UpdateUserCommandRequest request, CancellationToken cancellationToken)
        {
            if (!request.IsValid() || request.DataModel == null)
            {
                return new BaseResponse
                {
                    IsSuccessful = false,
                    Message = "Invalid Request",
                    Data = request.Notifications
                };
            }

            var user = await _userRepository.GetByIdAsync(request.Id);
            if (user == null)
            {
                return new BaseResponse
                {
                    IsSuccessful = false,
                    Message = "User not found",
                    Data = null
                };
            }

            user.Username = request.DataModel.Username;
            user.Password = request.DataModel.Password;
            user.Email = request.DataModel.Email;

            await _userRepository.UpdateAsync(user);
            
            return new BaseResponse
            {
                IsSuccessful = true,
                Message = "User updated successfully",
                Data = user
            };
        }
    }
}