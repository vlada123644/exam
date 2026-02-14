using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using TradeAutomation.Models;

namespace TradeAutomation.ViewModels
{
    public partial class DashboardViewModel : ViewModelBase
    {
        private string _pageTitle = "Панель управления";
        public string PageTitle
        {
            get => _pageTitle;
            set => SetProperty(ref _pageTitle, value);
        }

        private int _totalProducts;
        public int TotalProducts
        {
            get => _totalProducts;
            set => SetProperty(ref _totalProducts, value);
        }

        private int _totalOrders;
        public int TotalOrders
        {
            get => _totalOrders;
            set => SetProperty(ref _totalOrders, value);
        }

        private int _totalUsers;
        public int TotalUsers
        {
            get => _totalUsers;
            set => SetProperty(ref _totalUsers, value);
        }

        private User? _currentUser;

        public void SetCurrentUser(User? user)
        {
            _currentUser = user;
        }

        public override async Task InitializeAsync()
        {
            await LoadDashboardDataAsync();
        }

        private async Task LoadDashboardDataAsync()
        {
            IsBusy = true;
            await Task.Delay(500);

            TotalProducts = 42;
            TotalOrders = 15;
            TotalUsers = 3;

            StatusMessage = "Данные панели управления загружены";
            IsBusy = false;
        }

        [RelayCommand]
        private async Task RefreshAsync()
        {
            await LoadDashboardDataAsync();
        }
    }
}