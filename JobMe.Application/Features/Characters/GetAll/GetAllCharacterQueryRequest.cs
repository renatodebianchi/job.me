using Application.Requests;

namespace Features.Characters.GetAll
{
    public class GetAllCharacterQueryRequest : QuickRequest
    {
        public GetAllCharacterQueryRequest() : base()
        {
        }
        // Add any additional properties or methods if needed
        public override bool IsValid()
        {
            return true;
        }
    }

}