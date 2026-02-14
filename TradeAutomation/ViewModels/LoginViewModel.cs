using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using TradeAutomation.Models;
using TradeAutomation.Services;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.VisualBasic.Logging;

namespace TradeAutomation.ViewModels
{
    public partial class LoginViewModel : ViewModelBase
    {
        private readonly IAuthService _authService;
        private readonly IServiceProvider _serviceProvider;

        [ObservableProperty]
        private string _login = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        [ObservableProperty]
        private bool _hasError;

        public event EventHandler<User>? LoginSuccess;

        public LoginViewModel(IAuthService authService, IServiceProvider serviceProvider)
        {
            _authService = authService;
            _serviceProvider = serviceProvider;
        }

        [RelayCommand]
        private async Task LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Введите логин и пароль";
                HasError = true;
                return;
            }

            IsBusy = true;
            ErrorMessage = string.Empty;
            HasError = false;

            try
            {
                var user = await _authService.AuthenticateAsync(Login, Password);

                if (user != null)
                {
                    LoginSuccess?.Invoke(this, user);
                }
                else
                {
                    ErrorMessage = "Неверный логин или пароль";
                    HasError = true;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка: {ex.Message}";
                HasError = true;
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task LoginAsGuestAsync()
        {
            // Создаем гостевого пользователя
            var guestUser = new User
            {
                Id = -1,
                Login = "guest",
                FullName = "Гость",
                Role = new Role { Id = 3, Name = "Guest", Description = "Гость" },
                IsActive = true
            };

            LoginSuccess?.Invoke(this, guestUser);
            await Task.CompletedTask;
        }
    }
}