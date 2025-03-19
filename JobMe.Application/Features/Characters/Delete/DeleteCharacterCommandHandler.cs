using Application.Responses;
using Application.Handlers;
using Domain.Interfaces.Repositories;
using Domain.Entities;

namespace Features.Characters.Delete
{
    public class DeleteCharacterCommandHandler : QuickRequestHandler<DeleteCharacterCommandRequest>
    {
        IGenericRepository<Character> _characterRepository;
        public DeleteCharacterCommandHandler(IGenericRepository<Character> characterRepository)
        {
            _characterRepository = characterRepository;
        }

        public override async Task<BaseResponse> Handle(DeleteCharacterCommandRequest request, CancellationToken cancellationToken)
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
            if (Character == null)
            {
                return new BaseResponse
                {
                    IsSuccessful = false,
                    Message = "Character not found",
                    Data = null
                };
            }

            
            var deleted = await _characterRepository.DeleteAsync(Character.Id);
            
            if (deleted > 0)
            {
                return new BaseResponse
                {
                    IsSuccessful = true,
                    Message = "Character deleted successfully",
                    Data = Character
                };
            } else {
                return new BaseResponse
                {
                    IsSuccessful = false,
                    Message = "Character not deleted",
                    Data = null
                };
            }
        }
    }
}