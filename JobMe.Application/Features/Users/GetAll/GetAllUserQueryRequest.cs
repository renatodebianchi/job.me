using Application.Requests;

namespace Features.Users.GetAll
{
    public class GetAllUserQueryRequest : QuickRequest
    {
        // Add any additional properties or methods if needed
        public override bool IsValid()
        {
            return true;
        }
    }
}