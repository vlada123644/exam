using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Media.Imaging;
using TradeAutomation.Models;
using TradeAutomation.Services;

namespace TradeAutomation.ViewModels
{
    public partial class ProductEditViewModel : ViewModelBase
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IDialogService _dialogService;

        private Product? _originalProduct;
        private bool _isEditMode;
        private string _imagesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ProductImages");

        // РУЧНЫЕ СВОЙСТВА - БЕЗ АТРИБУТОВ
        private int _id;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string? _description;
        public string? Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private Category? _selectedCategory;
        public Category? SelectedCategory
        {
            get => _selectedCategory;
            set => SetProperty(ref _selectedCategory, value);
        }

        private Manufacturer? _selectedManufacturer;
        public Manufacturer? SelectedManufacturer
        {
            get => _selectedManufacturer;
            set => SetProperty(ref _selectedManufacturer, value);
        }

        private Supplier? _selectedSupplier;
        public Supplier? SelectedSupplier
        {
            get => _selectedSupplier;
            set => SetProperty(ref _selectedSupplier, value);
        }

        private decimal _price;
        public decimal Price
        {
            get => _price;
            set => SetProperty(ref _price, value);
        }

        private decimal? _discount;
        public decimal? Discount
        {
            get => _discount;
            set => SetProperty(ref _discount, value);
        }

        private int _quantityInStock;
        public int QuantityInStock
        {
            get => _quantityInStock;
            set => SetProperty(ref _quantityInStock, value);
        }

        private string _unitOfMeasure = "шт";
        public string UnitOfMeasure
        {
            get => _unitOfMeasure;
            set => SetProperty(ref _unitOfMeasure, value);
        }

        private string? _imagePath;
        public string? ImagePath
        {
            get => _imagePath;
            set => SetProperty(ref _imagePath, value);
        }

        private BitmapImage? _productImage;
        public BitmapImage? ProductImage
        {
            get => _productImage;
            set => SetProperty(ref _productImage, value);
        }

        private ObservableCollection<Category> _categories = new();
        public ObservableCollection<Category> Categories
        {
            get => _categories;
            set => SetProperty(ref _categories, value);
        }

        private ObservableCollection<Manufacturer> _manufacturers = new();
        public ObservableCollection<Manufacturer> Manufacturers
        {
            get => _manufacturers;
            set => SetProperty(ref _manufacturers, value);
        }

        private ObservableCollection<Supplier> _suppliers = new();
        public ObservableCollection<Supplier> Suppliers
        {
            get => _suppliers;
            set => SetProperty(ref _suppliers, value);
        }

        public ObservableCollection<string> UnitsOfMeasure { get; } = new()
        {
            "шт", "кг", "л", "м", "уп", "кор"
        };

        // Конструктор
        public ProductEditViewModel(
            IRepository<Product> productRepository,
            IDialogService dialogService)
        {
            _productRepository = productRepository;
            _dialogService = dialogService;

            if (!Directory.Exists(_imagesFolder))
                Directory.CreateDirectory(_imagesFolder);
        }

        public void InitializeForAdd(
            List<Category> categories,
            List<Manufacturer> manufacturers,
            List<Supplier> suppliers)
        {
            _isEditMode = false;
            _originalProduct = null;

            Categories = new ObservableCollection<Category>(categories);
            Manufacturers = new ObservableCollection<Manufacturer>(manufacturers);
            Suppliers = new ObservableCollection<Supplier>(suppliers);

            ClearForm();
        }

        public void InitializeForEdit(
            Product product,
            List<Category> categories,
            List<Manufacturer> manufacturers,
            List<Supplier> suppliers)
        {
            _isEditMode = true;
            _originalProduct = product;

            Categories = new ObservableCollection<Category>(categories);
            Manufacturers = new ObservableCollection<Manufacturer>(manufacturers);
            Suppliers = new ObservableCollection<Supplier>(suppliers);

            Id = product.Id;
            Name = product.Name;
            Description = product.Description;
            SelectedCategory = categories.FirstOrDefault(c => c.Id == product.CategoryId);
            SelectedManufacturer = manufacturers.FirstOrDefault(m => m.Id == product.ManufacturerId);
            SelectedSupplier = suppliers.FirstOrDefault(s => s.Id == product.SupplierId);
            Price = product.Price;
            Discount = product.Discount;
            QuantityInStock = product.QuantityInStock;
            UnitOfMeasure = product.UnitOfMeasure;
            ImagePath = product.ImagePath;

            LoadImage();
        }

        private void ClearForm()
        {
            Id = 0;
            Name = string.Empty;
            Description = null;
            SelectedCategory = Categories.FirstOrDefault();
            SelectedManufacturer = Manufacturers.FirstOrDefault();
            SelectedSupplier = Suppliers.FirstOrDefault();
            Price = 0;
            Discount = 0;
            QuantityInStock = 0;
            UnitOfMeasure = "шт";
            ImagePath = null;
            ProductImage = null;
        }

        private void LoadImage()
        {
            if (!string.IsNullOrEmpty(ImagePath) && File.Exists(ImagePath))
            {
                try
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(ImagePath, UriKind.Absolute);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.DecodePixelWidth = 300;
                    bitmap.DecodePixelHeight = 200;
                    bitmap.EndInit();
                    bitmap.Freeze();
                    ProductImage = bitmap;
                }
                catch
                {
                    ProductImage = null;
                }
            }
            else
            {
                ProductImage = null;
            }
        }

        [RelayCommand]
        private void SelectImage()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp",
                Title = "Выберите изображение товара"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    if (_isEditMode && !string.IsNullOrEmpty(ImagePath) && File.Exists(ImagePath))
                    {
                        File.Delete(ImagePath);
                    }

                    var fileName = $"product_{DateTime.Now:yyyyMMdd_HHmmss}{Path.GetExtension(openFileDialog.FileName)}";
                    var newImagePath = Path.Combine(_imagesFolder, fileName);

                    File.Copy(openFileDialog.FileName, newImagePath);

                    ImagePath = newImagePath;
                    LoadImage();
                }
                catch (Exception ex)
                {
                    _ = _dialogService.ShowMessageAsync("Ошибка", $"Не удалось загрузить изображение: {ex.Message}");
                }
            }
        }

        [RelayCommand]
        private void RemoveImage()
        {
            if (!string.IsNullOrEmpty(ImagePath) && File.Exists(ImagePath))
            {
                File.Delete(ImagePath);
            }

            ImagePath = null;
            ProductImage = null;
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                await _dialogService.ShowMessageAsync("Ошибка", "Введите наименование товара");
                return;
            }

            if (SelectedCategory == null)
            {
                await _dialogService.ShowMessageAsync("Ошибка", "Выберите категорию");
                return;
            }

            if (SelectedManufacturer == null)
            {
                await _dialogService.ShowMessageAsync("Ошибка", "Выберите производителя");
                return;
            }

            if (SelectedSupplier == null)
            {
                await _dialogService.ShowMessageAsync("Ошибка", "Выберите поставщика");
                return;
            }

            if (Price < 0)
            {
                await _dialogService.ShowMessageAsync("Ошибка", "Цена не может быть отрицательной");
                return;
            }

            if (QuantityInStock < 0)
            {
                await _dialogService.ShowMessageAsync("Ошибка", "Количество не может быть отрицательным");
                return;
            }

            try
            {
                if (_isEditMode && _originalProduct != null)
                {
                    _originalProduct.Name = Name;
                    _originalProduct.Description = Description;
                    _originalProduct.CategoryId = SelectedCategory.Id;
                    _originalProduct.ManufacturerId = SelectedManufacturer.Id;
                    _originalProduct.SupplierId = SelectedSupplier.Id;
                    _originalProduct.Price = Price;
                    _originalProduct.Discount = Discount;
                    _originalProduct.QuantityInStock = QuantityInStock;
                    _originalProduct.UnitOfMeasure = UnitOfMeasure;
                    _originalProduct.ImagePath = ImagePath;

                    await _productRepository.UpdateAsync(_originalProduct);
                }
                else
                {
                    var newProduct = new Product
                    {
                        Name = Name,
                        Description = Description,
                        CategoryId = SelectedCategory.Id,
                        ManufacturerId = SelectedManufacturer.Id,
                        SupplierId = SelectedSupplier.Id,
                        Price = Price,
                        Discount = Discount,
                        QuantityInStock = QuantityInStock,
                        UnitOfMeasure = UnitOfMeasure,
                        ImagePath = ImagePath,
                        CreatedAt = DateTime.Now
                    };

                    await _productRepository.AddAsync(newProduct);
                }

                CloseDialog(true);
            }
            catch (Exception ex)
            {
                await _dialogService.ShowMessageAsync("Ошибка", $"Не удалось сохранить товар: {ex.Message}");
            }
        }

        [RelayCommand]
        private void Cancel()
        {
            if (!_isEditMode && !string.IsNullOrEmpty(ImagePath) && File.Exists(ImagePath))
            {
                File.Delete(ImagePath);
            }

            CloseDialog(false);
        }

        public event EventHandler<bool>? DialogClosed;

        private void CloseDialog(bool result)
        {
            DialogClosed?.Invoke(this, result);
        }
    }
}
