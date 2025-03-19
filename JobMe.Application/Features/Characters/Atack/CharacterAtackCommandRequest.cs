using Application.Requests.Commands;
using Domain.Entities;

namespace Features.Characters.Update
{
    public class CharacterAtackCommandRequest : QuickCommandRequest<Character>
    {
        public CharacterAtackCommandRequest(int atackerId, int defenderId) : base()
        {
            AtackerId = atackerId;
            DefenderId = defenderId;
        }
        public int AtackerId { get; set; }
        public int DefenderId { get; set; }
        public override bool IsValid()
        {
            if (AtackerId <= 0) {
                Notifications.Add("Invalid Atacker Id", "Atacker Id is required");
                return false;
            }
            if (DefenderId <= 0) {
                Notifications.Add("Invalid Defender Id", "Defender Id is required");
                return false;
            }
            
            return true;
        }
    }
    
}