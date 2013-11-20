using Bloghost.Core.Entities;

namespace Bloghost.Core.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        User GetUserByLogin(string name);

        User GetUserByLoginAndPassword(string login, string password);
    }
}
