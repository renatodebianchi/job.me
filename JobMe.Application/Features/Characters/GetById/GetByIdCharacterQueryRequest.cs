using Application.Requests;

namespace Features.Characters.GetById
{
    public class GetByIdCharacterQueryRequest : QuickRequest
    {
        public GetByIdCharacterQueryRequest() : base()
        {
        }
        public int Id { get; set; }
        // Add any additional properties or methods if needed
        public override bool IsValid()
        {
            if (Id <= 0)
            {
                Notifications.Add("Id", "Id must be greater than 0");
                return false;
            }
            return true;
        }
    }

}