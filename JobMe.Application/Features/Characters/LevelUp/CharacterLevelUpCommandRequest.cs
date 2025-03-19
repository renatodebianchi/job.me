using Application.Requests.Commands;
using Domain.Entities;

namespace Features.Characters.Update
{
    public class CharacterLevelUpCommandRequest : QuickCommandRequest<Character>
    {
        public CharacterLevelUpCommandRequest(Character character) : base()
        {
            DataModel = character;
            if (character != null)
            {
                Id = character.Id;
            }
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