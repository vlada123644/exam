using System.Collections.Generic;
using System.Threading.Tasks;
using TradeAutomation.Models;

namespace TradeAutomation.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProductsWithDetailsAsync();
        Task<Product?> GetProductWithDetailsAsync(int id);
        Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm);
        Task<IEnumerable<Product>> FilterByCategoryAsync(int categoryId);
        Task<IEnumerable<Product>> SortByPriceAsync(bool ascending = true);
        Task<bool> UpdateStockAsync(int productId, int quantity);
        Task<byte[]?> ExportToExcelAsync(); // Изменили на nullable
    }

    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Manufacturer> _manufacturerRepository;
        private readonly IRepository<Supplier> _supplierRepository;

        public ProductService(
            IRepository<Product> productRepository,
            IRepository<Category> categoryRepository,
            IRepository<Manufacturer> manufacturerRepository,
            IRepository<Supplier> supplierRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _manufacturerRepository = manufacturerRepository;
            _supplierRepository = supplierRepository;
        }

        public async Task<IEnumerable<Product>> GetProductsWithDetailsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return products.OrderBy(p => p.Name);
        }

        public async Task<Product?> GetProductWithDetailsAsync(int id)
        {
            return await _productRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetProductsWithDetailsAsync();

            searchTerm = searchTerm.ToLower();
            var products = await _productRepository.FindAsync(p =>
                p.Name.ToLower().Contains(searchTerm) ||
                (p.Description != null && p.Description.ToLower().Contains(searchTerm)));

            return products.OrderBy(p => p.Name);
        }

        public async Task<IEnumerable<Product>> FilterByCategoryAsync(int categoryId)
        {
            var products = await _productRepository.FindAsync(p => p.CategoryId == categoryId);
            return products.OrderBy(p => p.Name);
        }

        public async Task<IEnumerable<Product>> SortByPriceAsync(bool ascending = true)
        {
            var products = await GetProductsWithDetailsAsync();
            return ascending
                ? products.OrderBy(p => p.Price)
                : products.OrderByDescending(p => p.Price);
        }

        public async Task<bool> UpdateStockAsync(int productId, int quantity)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null) return false;

            product.QuantityInStock = quantity;
            await _productRepository.UpdateAsync(product);
            return true;
        }

        public async Task<byte[]?> ExportToExcelAsync()
        {
            // ВРЕМЕННО: пока без ClosedXML
            await Task.Delay(100);
            return null;

            /* ПОЗЖЕ РАСКОММЕНТИРОВАТЬ:
            var products = await GetProductsWithDetailsAsync();
            
            using var workbook = new ClosedXML.Excel.XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Products");

            worksheet.Cell(1, 1).Value = "ID";
            worksheet.Cell(1, 2).Value = "Наименование";
            worksheet.Cell(1, 3).Value = "Категория";
            worksheet.Cell(1, 4).Value = "Цена";
            worksheet.Cell(1, 5).Value = "Количество";

            int row = 2;
            foreach (var product in products)
            {
                worksheet.Cell(row, 1).Value = product.Id;
                worksheet.Cell(row, 2).Value = product.Name;
                worksheet.Cell(row, 3).Value = product.Category?.Name ?? "";
                worksheet.Cell(row, 4).Value = product.Price;
                worksheet.Cell(row, 5).Value = product.QuantityInStock;
                row++;
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
            */
        }
    }
}