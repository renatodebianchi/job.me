using Application.Responses;
using Domain.Interfaces.Repositories;
using Domain.Entities;
using Application.Handlers;

namespace Features.Characters.GetAll
{
    public class GetAllCharacterQueryHandler : QuickRequestHandler<GetAllCharacterQueryRequest>
    {
        IGenericRepository<Character> _characterRepository;
        public GetAllCharacterQueryHandler(IGenericRepository<Character> characterRepository)
        {
            _characterRepository = characterRepository;
        }

        public override async Task<BaseResponse> Handle(GetAllCharacterQueryRequest request, CancellationToken cancellationToken)
        {
            var allCharacters = await _characterRepository.GetAllAsync();
            var response = new BaseResponse
            {
                IsSuccessful = true,
                Message = "Hello, World!",
                Data = allCharacters
            };
            return response;
        }
    }
}