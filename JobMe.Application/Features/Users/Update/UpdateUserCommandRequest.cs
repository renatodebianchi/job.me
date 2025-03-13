using Application.Requests.Commands;
using Domain.Entities;

namespace Features.Users.Update
{
    public class UpdateUserCommandRequest : QuickCommandRequest<User>
    {
        public UpdateUserCommandRequest() : base()
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
            if (string.IsNullOrEmpty(DataModel.Username)) {
                Notifications.Add("Invalid Username", "Username is required");
                isValid = false;
            }

            if (string.IsNullOrEmpty(DataModel.Password)) {
                Notifications.Add("Invalid Password", "Password is required");
                isValid = false;
            }

            if (string.IsNullOrEmpty(DataModel.Email)) {
                Notifications.Add("Invalid Email", "Email is required");
                isValid = false;
            }

            return isValid;
        }
    }
    
}