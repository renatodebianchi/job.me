using Application.Requests.Commands;
using Domain.Entities;

namespace Features.Characters.Update
{
    public class UpdateCharacterCommandRequest : QuickCommandRequest<Character>
    {
        public UpdateCharacterCommandRequest() : base()
        {
        }
        public int Id { get; set; }
        public override bool IsValid()
        {
            if (Id <= 0) {
                Notifications.Add("Invalid Id", "Id is required");
                return false;
            }

            if (DataModel == null) {
                Notifications.Add("Invalid Model", "Model is null");
                return false;
            }

            bool isValid = true;
            if (string.IsNullOrEmpty(DataModel.Name)) {
                Notifications.Add("Invalid Character Name", "Character Name is required");
                isValid = false;
            }

            return isValid;
        }
    }
    
}