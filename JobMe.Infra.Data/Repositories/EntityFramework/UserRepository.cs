using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infra.Data.Contexts;

namespace Infra.Data.Repositories.EntityFramework
{
    public class UserRepository : BaseRepositoryEntityFramework<User>
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        // Add any additional methods specific to UserRepository here
    }
}