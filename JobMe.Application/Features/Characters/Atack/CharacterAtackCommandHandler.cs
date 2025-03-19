using Application.Responses;
using Application.Handlers;
using Domain.Interfaces.Repositories;
using Domain.Entities;
using Domain.Extensions;

namespace Features.Characters.Update
{
    public class CharacterAtackCommandHandler : QuickRequestHandler<CharacterAtackCommandRequest>
    {
        IGenericRepository<Character> _characterRepository;
        public CharacterAtackCommandHandler(IGenericRepository<Character> characterRepository)
        {
            _characterRepository = characterRepository;
        }

        public override async Task<BaseResponse> Handle(CharacterAtackCommandRequest request, CancellationToken cancellationToken)
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

            var atacker = await _characterRepository.GetByIdAsync(request.AtackerId);
            if (atacker == null)
            {
                return new BaseResponse
                {
                    IsSuccessful = false,
                    Message = "Atacker Character not found",
                    Data = null
                };
            }

            var defender = await _characterRepository.GetByIdAsync(request.DefenderId);
            if (defender == null)
            {
                return new BaseResponse
                {
                    IsSuccessful = false,
                    Message = "Defender Character not found",
                    Data = null
                };
            }

            var damage = atacker.CalculatePhysicalAtackDamage(defender).WithCriticalChance();
            
            if (damage > 0) {
                defender.TakeDamage(damage);
                await _characterRepository.UpdateAsync(defender);

                atacker.LevelUp();
                await _characterRepository.UpdateAsync(atacker);
            }
            
            return new BaseResponse
            {
                IsSuccessful = true,
                Message = "Atack computed successfully",
                Data = new { atacker, defender, damage }
            };
        }
    }
}