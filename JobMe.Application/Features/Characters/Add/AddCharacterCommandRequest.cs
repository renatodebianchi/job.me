using Application.Requests.Commands;
using Domain.Entities;

namespace Features.Characters.Add
{
    public class AddCharacterCommandRequest : QuickCommandRequest<Character>
    {
        public AddCharacterCommandRequest(Character character) : base()
        {
            DataModel = character;
        }
        public override bool IsValid()
        {
            if (DataModel == null) {
                Notifications.Add("Invalid Model", "Model is null");
                return false;
            }

            bool isValid = true;
            if (string.IsNullOrEmpty(DataModel.Name)) {
                Notifications.Add("Invalid Character name", "Character name is required");
                isValid = false;
            }

            return isValid;
        }
    }

}