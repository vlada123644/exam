using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;



using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using TradeAutomation.Models;
using TradeAutomation.Services;

namespace TradeAutomation.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly IAuthService _authService;
        private readonly IServiceProvider _serviceProvider;

        [ObservableProperty]
        private ViewModelBase? _currentViewModel;

        [ObservableProperty]
        private User? _currentUser;

        [ObservableProperty]
        private string _windowTitle = "Trade Automation System";

        public MainWindowViewModel(IAuthService authService, IServiceProvider serviceProvider)
        {
            _authService = authService;
            _serviceProvider = serviceProvider;

            // Инициализация - показываем окно входа
            ShowLoginCommand.ExecuteAsync(null);
        }

        [RelayCommand]
        private async Task ShowLogin()
        {
            var loginVM = _serviceProvider.GetRequiredService<LoginViewModel>();
            loginVM.LoginSuccess += OnLoginSuccess;
            CurrentViewModel = loginVM;
            await loginVM.InitializeAsync();
        }

        [RelayCommand]
        private async Task ShowProducts()
        {
            var productsVM = _serviceProvider.GetRequiredService<ProductListViewModel>();
            productsVM.SetCurrentUser(_currentUser);
            CurrentViewModel = productsVM;
            await productsVM.InitializeAsync();
        }

        [RelayCommand]
        private async Task ShowOrders()
        {
            var ordersVM = _serviceProvider.GetRequiredService<OrderListViewModel>();
            ordersVM.SetCurrentUser(_currentUser);
            CurrentViewModel = ordersVM;
            await ordersVM.InitializeAsync();
        }

        [RelayCommand]
        private async Task ShowDashboard()
        {
            var dashboardVM = _serviceProvider.GetRequiredService<DashboardViewModel>();
            dashboardVM.SetCurrentUser(_currentUser);
            CurrentViewModel = dashboardVM;
            await dashboardVM.InitializeAsync();
        }

        [RelayCommand]
        private async Task Logout()
        {
            _authService.Logout();
            _currentUser = null;
            await ShowLogin();
        }

        private async void OnLoginSuccess(object? sender, User user)
        {
            _currentUser = user;
            await ShowDashboard();
        }

        public bool CanUserAccessProducts => _currentUser != null;
        public bool CanUserAccessOrders => _currentUser != null &&
            (_currentUser.Role.Name == "Admin" || _currentUser.Role.Name == "Manager");
        public bool IsAdmin => _currentUser?.Role.Name == "Admin";
    }
}