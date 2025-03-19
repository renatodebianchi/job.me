using Application.Responses;
using Application.Handlers;
using Domain.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums;

namespace Features.Characters.Add
{
    public class AddCharacterCommandHandler : QuickRequestHandler<AddCharacterCommandRequest>
    {
        IGenericRepository<Character> _characterRepository;
        public AddCharacterCommandHandler(IGenericRepository<Character> characterRepository)
        {
            _characterRepository = characterRepository;
        }

        public override async Task<BaseResponse> Handle(AddCharacterCommandRequest request, CancellationToken cancellationToken)
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

            var newCharacter = new Character(request.DataModel.Name, 1) 
            {
                MaxHealth = 100, 
                Health = 100,
                PhysicalAtack = 10,
                PhysicalDefense = 10,
                Speed = 10,
                Status = CharacterStatus.Active
            };

            await _characterRepository.AddAsync(newCharacter);
            
            return new BaseResponse
            {
                IsSuccessful = true,
                Message = "Character added successfully",
                Data = newCharacter
            };
        }
    }
}