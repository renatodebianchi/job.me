using Application.Requests;

namespace Features.Users.GetById
{
    public class GetByIdUserQueryRequest : QuickRequest
    {
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