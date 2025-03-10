using Application.Responses;
using Application.Handlers;
using Domain.Interfaces.Repositories;
using Domain.Entities;

namespace Features.Users.Delete
{
    public class DeleteUserCommandHandler : QuickRequestHandler<DeleteUserCommandRequest>
    {
        IGenericRepository<User> _userRepository;
        public DeleteUserCommandHandler(IGenericRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public override async Task<BaseResponse> Handle(DeleteUserCommandRequest request, CancellationToken cancellationToken)
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

            
            var deleted = await _userRepository.DeleteAsync(user.Id);
            
            if (deleted > 0)
            {
                return new BaseResponse
                {
                    IsSuccessful = true,
                    Message = "User deleted successfully",
                    Data = user
                };
            } else {
                return new BaseResponse
                {
                    IsSuccessful = false,
                    Message = "User not deleted",
                    Data = null
                };
            }
        }
    }
}