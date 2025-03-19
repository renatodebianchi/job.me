using Application.Responses;
using Domain.Interfaces.Repositories;
using Domain.Entities;
using Application.Handlers;

namespace Features.Characters.GetById
{
    public class GetByIdCharacterQueryHandler : QuickRequestHandler<GetByIdCharacterQueryRequest>
    {
        IGenericRepository<Character> _characterRepository;
        public GetByIdCharacterQueryHandler(IGenericRepository<Character> CharacterRepository)
        {
            _characterRepository = CharacterRepository;
        }

        public override async Task<BaseResponse> Handle(GetByIdCharacterQueryRequest request, CancellationToken cancellationToken)
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

            var Character = await _characterRepository.GetByIdAsync(request.Id);
            var response = new BaseResponse
            {
                IsSuccessful = true,
                Message = "Hello, World!",
                Data = Character
            };
            return response;
        }
    }
}