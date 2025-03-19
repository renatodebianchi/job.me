using Application.Requests.Commands;
using Domain.Entities;

namespace Features.Characters.Delete
{
    public class DeleteCharacterCommandRequest : QuickCommandRequest<Character>
    {
        public DeleteCharacterCommandRequest() : base()
        {
        }
        public int Id { get; set; }
        public override bool IsValid()
        {
            if (Id <= 0) {
                Notifications.Add("Invalid Id", "Id is required");
                return false;
            }

            return true;
        }
    }
    
}