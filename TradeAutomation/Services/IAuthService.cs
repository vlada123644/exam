using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using TradeAutomation.Models;

namespace TradeAutomation.Services
{
    public interface IAuthService
    {
        Task<User?> AuthenticateAsync(string login, string password);
        Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword);
        Task<User?> GetCurrentUserAsync();
        void Logout();
    }

    public class AuthService : IAuthService
    {
        private readonly IRepository<User> _userRepository;
        private User? _currentUser;

        public AuthService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> AuthenticateAsync(string login, string password)
        {
            // В реальном приложении пароль должен быть хеширован
            var user = (await _userRepository.FindAsync(u =>
                u.Login == login && u.Password == password && u.IsActive))
                .FirstOrDefault();

            if (user != null)
            {
                _currentUser = user;
            }

            return user;
        }

        public async Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null || user.Password != oldPassword) return false;

            user.Password = newPassword;
            await _userRepository.UpdateAsync(user);
            return true;
        }

        public Task<User?> GetCurrentUserAsync()
        {
            return Task.FromResult(_currentUser);
        }

        public void Logout()
        {
            _currentUser = null;
        }
    }
}
