using Application.Responses;
using Domain.Interfaces.Repositories;
using Domain.Entities;
using Application.Handlers;

namespace Features.Users.GetAll
{
    public class GetAllUserQueryHandler : QuickRequestHandler<GetAllUserQueryRequest>
    {
        IGenericRepository<User> _userRepository;
        public GetAllUserQueryHandler(IGenericRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public override async Task<BaseResponse> Handle(GetAllUserQueryRequest request, CancellationToken cancellationToken)
        {
            var allUsers = await _userRepository.GetAllAsync();
            var response = new BaseResponse
            {
                IsSuccessful = true,
                Message = "Hello, World!",
                Data = allUsers
            };
            return response;
        }
    }
}