using System;
using System.Collections.Generic;
using Bloghost.Core.Entities;
using Bloghost.Core.Repositories;
using Bloghost.Core.Services.Interfaces;

namespace Bloghost.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [UnitOfWork]
        public IEnumerable<User> GetUserList()
        {
            return _userRepository.GetAll();
        }

        [UnitOfWork]
        public User GetUser(Guid id)
        {
            return _userRepository.Get(id);
        }

        [UnitOfWork]
        public User GetUser(string login, string password)
        {
            return _userRepository.GetUserByLoginAndPassword(login, password);
        }

        [UnitOfWork]
        public User GetUser(string login)
        {
            return _userRepository.GetUserByLogin(login);
        }

        [UnitOfWork]
        public void AddNewUser(User user)
        {
            _userRepository.Create(user);
        }

        [UnitOfWork]
        public void EditUser(User user)
        {
            _userRepository.Update(user);
        }

        [UnitOfWork]
        public void DeleteUser(Guid id)
        {
            _userRepository.Delete(id);
        }

        public void Save(User user)
        {
            if (user.ID != Guid.Empty)
            {
                _userRepository.Update(user);
            }
            else
            {
                _userRepository.Create(user);
            }
        }
    }
}
