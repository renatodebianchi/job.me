using Application.Responses;
using Domain.Interfaces.Repositories;
using Domain.Entities;
using Application.Handlers;

namespace Features.Users.GetById
{
    public class GetByIdUserQueryHandler : QuickRequestHandler<GetByIdUserQueryRequest>
    {
        IGenericRepository<User> _userRepository;
        public GetByIdUserQueryHandler(IGenericRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public override async Task<BaseResponse> Handle(GetByIdUserQueryRequest request, CancellationToken cancellationToken)
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
            var response = new BaseResponse
            {
                IsSuccessful = true,
                Message = "Hello, World!",
                Data = user
            };
            return response;
        }
    }
}