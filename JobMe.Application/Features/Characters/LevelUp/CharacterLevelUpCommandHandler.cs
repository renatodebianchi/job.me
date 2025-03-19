using Application.Responses;
using Application.Handlers;
using Domain.Interfaces.Repositories;
using Domain.Entities;
using Domain.Extensions;

namespace Features.Characters.Update
{
    public class CharacterLevelUpCommandHandler : QuickRequestHandler<CharacterLevelUpCommandRequest>
    {
        IGenericRepository<Character> _characterRepository;
        public CharacterLevelUpCommandHandler(IGenericRepository<Character> characterRepository)
        {
            _characterRepository = characterRepository;
        }

        public override async Task<BaseResponse> Handle(CharacterLevelUpCommandRequest request, CancellationToken cancellationToken)
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

            var character = await _characterRepository.GetByIdAsync(request.Id);
            if (character == null)
            {
                return new BaseResponse
                {
                    IsSuccessful = false,
                    Message = "Character not found",
                    Data = null
                };
            }

            character.LevelUp();
            await _characterRepository.UpdateAsync(character);
            
            return new BaseResponse
            {
                IsSuccessful = true,
                Message = "Character updated successfully",
                Data = character
            };
        }
    }
}