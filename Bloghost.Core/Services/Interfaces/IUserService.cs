using System.Collections.Generic;
using Bloghost.Core.Entities;

namespace Bloghost.Core.Services.Interfaces
{
    public interface IUserService
    {
        IEnumerable<User> GetUserList();

        User GetUser(System.Guid id);

        User GetUser(string login, string password);

        User GetUser(string login);

        void AddNewUser(User user);

        void EditUser(User user);

        void DeleteUser(System.Guid id);

        void Save(User user);
    }
}
