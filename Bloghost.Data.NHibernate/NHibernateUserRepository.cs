using System.Linq;
using Bloghost.Core.Entities;
using Bloghost.Core.Repositories;
using NHibernate.Linq;

namespace Bloghost.Data.NHibernate
{
    public class NHibernateUserRepository : NHibernateRepository<User>, IUserRepository
    {
        public User GetUserByLogin(string login)
        {
            var query = Session.Query<User>()
                .Where(x => x.Login == login).ToArray();
            return query.Length == 0 ? null : query[0];
        }


        public User GetUserByLoginAndPassword(string login, string password)
        {
            var query = Session.Query<User>()
                .Where(x => x.Login == login).Where(x => x.Password == password).ToArray();
            return query.Length == 0 ? null : query[0];
        }
    }
}
