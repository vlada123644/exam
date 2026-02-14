using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using TradeAutomation.Models;

namespace TradeAutomation.ViewModels
{
    public partial class OrderListViewModel : ViewModelBase
    {
        private string _pageTitle = "Управление заказами";
        public string PageTitle
        {
            get => _pageTitle;
            set => SetProperty(ref _pageTitle, value);
        }

        private User? _currentUser;

        public void SetCurrentUser(User? user)
        {
            _currentUser = user;
        }

        public override Task InitializeAsync()
        {
            StatusMessage = "Модуль заказов в разработке";
            return Task.CompletedTask;
        }

        [RelayCommand]
        private Task RefreshAsync()
        {
            StatusMessage = "Данные обновлены";
            return Task.CompletedTask;
        }
    }
}
