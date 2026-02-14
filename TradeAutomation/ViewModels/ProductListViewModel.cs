using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using TradeAutomation.Models;
using TradeAutomation.Services;
using System;

namespace TradeAutomation.ViewModels
{
    public partial class ProductListViewModel : ViewModelBase
    {
        private readonly IProductService _productService;

        // УБИРАЕМ [ObservableProperty] - делаем ручные свойства
        private ObservableCollection<Product> _products = new();
        public ObservableCollection<Product> Products
        {
            get => _products;
            set => SetProperty(ref _products, value);
        }

        private Product? _selectedProduct;
        public Product? SelectedProduct
        {
            get => _selectedProduct;
            set => SetProperty(ref _selectedProduct, value);
        }

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        private bool _isAdmin;
        public bool IsAdmin
        {
            get => _isAdmin;
            set => SetProperty(ref _isAdmin, value);
        }

        private bool _isManager;
        public bool IsManager
        {
            get => _isManager;
            set => SetProperty(ref _isManager, value);
        }

        // Конструктор
        public ProductListViewModel(IProductService productService)
        {
            _productService = productService;
        }

        public void SetCurrentUser(User? user)
        {
            if (user != null)
            {
                IsAdmin = user.Role?.Name == "Admin";
                IsManager = IsAdmin || user.Role?.Name == "Manager";
            }
        }

        public override async Task InitializeAsync()
        {
            await LoadProductsAsync();
        }

        private async Task LoadProductsAsync()
        {
            IsBusy = true;
            try
            {
                var products = await _productService.GetProductsWithDetailsAsync();
                Products = new ObservableCollection<Product>(products);
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task RefreshAsync()
        {
            await LoadProductsAsync();
            StatusMessage = "Данные обновлены";
        }

        [RelayCommand]
        private Task AddProductAsync()
        {
            StatusMessage = "Функция добавления временно недоступна";
            return Task.CompletedTask;
        }

        [RelayCommand]
        private Task EditProductAsync()
        {
            StatusMessage = "Функция редактирования временно недоступна";
            return Task.CompletedTask;
        }

        [RelayCommand]
        private Task DeleteProductAsync()
        {
            StatusMessage = "Функция удаления временно недоступна";
            return Task.CompletedTask;
        }

        [RelayCommand]
        private Task ExportToExcelAsync()
        {
            StatusMessage = "Экспорт временно недоступен";
            return Task.CompletedTask;
        }
    }
}
