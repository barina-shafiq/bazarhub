using Microsoft.AspNetCore.Http;
using WebProject.Models;
using WebProject.ViewModels;

namespace WebProject.Interfaces;

public interface IProductService
{
    Task<HomeViewModel> GetHomeAsync(string? userId);
    Task<ProductListViewModel> SearchAsync(ProductListViewModel filters);
    Task<ProductDetailsViewModel?> GetDetailsAsync(int id);
    Task<IReadOnlyList<Category>> GetCategoriesAsync();
    Task<ProductFormViewModel> BuildFormAsync(int? id = null);
    Task<int> SaveProductAsync(ProductFormViewModel model, IReadOnlyList<IFormFile> images);
    Task DeleteProductAsync(int id);
    Task DeleteImageAsync(int imageId);
    Task AddReviewAsync(int productId, string userId, ReviewInputModel model);
}

public interface ICartService
{
    Task<CartViewModel> GetCartAsync(string userId);
    Task AddAsync(string userId, int productId, int quantity);
    Task UpdateAsync(string userId, int productId, int quantity);
    Task RemoveAsync(string userId, int productId);
    Task ClearAsync(string userId);
}

public interface IWishlistService
{
    Task<IReadOnlyList<WishlistItem>> GetAsync(string userId);
    Task ToggleAsync(string userId, int productId);
    Task RemoveAsync(string userId, int productId);
}

public interface IOrderService
{
    Task<Order> CheckoutAsync(string userId, CheckoutViewModel model);
    Task<IReadOnlyList<Order>> GetUserOrdersAsync(string userId);
    Task<Order?> GetOrderDetailsAsync(int id, string? userId = null);
    Task<IReadOnlyList<Order>> GetAllOrdersAsync();
    Task UpdateStatusAsync(int id, OrderStatus status);
}

public interface IReportService
{
    Task<DashboardViewModel> GetDashboardAsync();
}
