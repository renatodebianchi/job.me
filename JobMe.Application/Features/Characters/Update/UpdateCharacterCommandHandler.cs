using Application.Responses;
using Application.Handlers;
using Domain.Interfaces.Repositories;
using Domain.Entities;
using Domain.Extensions;

namespace Features.Characters.Update
{
    public class UpdateCharacterCommandHandler : QuickRequestHandler<UpdateCharacterCommandRequest>
    {
        IGenericRepository<Character> _characterRepository;
        public UpdateCharacterCommandHandler(IGenericRepository<Character> characterRepository)
        {
            _characterRepository = characterRepository;
        }

        public override async Task<BaseResponse> Handle(UpdateCharacterCommandRequest request, CancellationToken cancellationToken)
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

            character.SpreadNotNull(request.DataModel);
            character.Id = request.Id;

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