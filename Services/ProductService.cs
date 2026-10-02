using Microsoft.EntityFrameworkCore;
using WebProject.Data;
using WebProject.Interfaces;
using WebProject.Models;
using WebProject.ViewModels;

namespace WebProject.Services;

public class ProductService(ApplicationDbContext context, IWebHostEnvironment environment) : IProductService
{
    public async Task<HomeViewModel> GetHomeAsync(string? userId)
    {
        var products = ProductQuery();
        return new HomeViewModel
        {
            Categories = await context.Categories.Where(c => c.IsActive).ToListAsync(),
            FeaturedProducts = await products.Where(p => p.IsFeatured).Take(8).ToListAsync(),
            TrendingProducts = await products.Where(p => p.IsTrending).Take(8).ToListAsync(),
            NewArrivals = await products.OrderByDescending(p => p.CreatedAt).Take(8).ToListAsync(),
            RecommendedProducts = await GetRecommendedAsync(userId)
        };
    }

    public async Task<ProductListViewModel> SearchAsync(ProductListViewModel filters)
    {
        var query = ProductQuery();

        if (!string.IsNullOrWhiteSpace(filters.Search))
        {
            query = query.Where(p => p.Name.Contains(filters.Search) || p.Brand.Contains(filters.Search));
        }

        if (filters.CategoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == filters.CategoryId);
        }

        if (filters.MinPrice.HasValue)
        {
            query = query.Where(p => (p.DiscountPrice ?? p.Price) >= filters.MinPrice.Value);
        }

        if (filters.MaxPrice.HasValue)
        {
            query = query.Where(p => (p.DiscountPrice ?? p.Price) <= filters.MaxPrice.Value);
        }

        query = filters.Sort switch
        {
            "price_asc" => query.OrderBy(p => p.DiscountPrice ?? p.Price),
            "price_desc" => query.OrderByDescending(p => p.DiscountPrice ?? p.Price),
            "newest" => query.OrderByDescending(p => p.CreatedAt),
            _ => query.OrderByDescending(p => p.CreatedAt)
        };

        var count = await query.CountAsync();
        filters.Page = Math.Max(1, filters.Page);
        filters.TotalPages = (int)Math.Ceiling(count / (double)filters.PageSize);
        filters.Products = await query.Skip((filters.Page - 1) * filters.PageSize).Take(filters.PageSize).ToListAsync();
        filters.Categories = await context.Categories.Where(c => c.IsActive).ToListAsync();
        return filters;
    }

    public async Task<ProductDetailsViewModel?> GetDetailsAsync(int id)
    {
        var product = await ProductQuery().FirstOrDefaultAsync(p => p.Id == id);
        if (product is null)
        {
            return null;
        }

        var related = await ProductQuery()
            .Where(p => p.Id != id && p.CategoryId == product.CategoryId)
            .Take(4)
            .ToListAsync();

        return new ProductDetailsViewModel { Product = product, RelatedProducts = related };
    }

    public async Task<IReadOnlyList<Category>> GetCategoriesAsync() =>
        await context.Categories.OrderBy(c => c.Name).ToListAsync();

    public async Task<ProductFormViewModel> BuildFormAsync(int? id = null)
    {
        var model = new ProductFormViewModel { Categories = await GetCategoriesAsync() };
        if (id is null)
        {
            return model;
        }

        var product = await context.Products.Include(p => p.Images).FirstAsync(p => p.Id == id);
        model.Id = product.Id;
        model.Name = product.Name;
        model.Brand = product.Brand;
        model.Description = product.Description;
        model.Price = product.Price;
        model.DiscountPrice = product.DiscountPrice;
        model.StockQuantity = product.StockQuantity;
        model.CategoryId = product.CategoryId;
        model.IsFeatured = product.IsFeatured;
        model.IsTrending = product.IsTrending;
        model.ExistingImages = product.Images.ToList();
        return model;
    }

    public async Task<int> SaveProductAsync(ProductFormViewModel model, IReadOnlyList<IFormFile> images)
    {
        Product product;
        if (model.Id == 0)
        {
            product = new Product();
            await context.Products.AddAsync(product);
        }
        else
        {
            product = await context.Products.Include(p => p.Images).FirstAsync(p => p.Id == model.Id);
        }

        product.Name = model.Name;
        product.Brand = model.Brand;
        product.Description = model.Description;
        product.Price = model.Price;
        product.DiscountPrice = model.DiscountPrice;
        product.StockQuantity = model.StockQuantity;
        product.CategoryId = model.CategoryId;
        product.IsFeatured = model.IsFeatured;
        product.IsTrending = model.IsTrending;

        await SaveImagesAsync(product, images);
        await context.SaveChangesAsync();
        return product.Id;
    }

    public async Task DeleteProductAsync(int id)
    {
        var product = await context.Products.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == id);
        if (product is not null)
        {
            foreach (var image in product.Images.Where(i => i.ImagePath.StartsWith("/uploads/", StringComparison.OrdinalIgnoreCase)))
            {
                DeleteLocalFile(image.ImagePath);
            }

            context.Products.Remove(product);
            await context.SaveChangesAsync();
        }
    }

    public async Task DeleteImageAsync(int imageId)
    {
        var product = await context.Products
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Images.Any(i => i.Id == imageId));

        if (product is null)
        {
            return;
        }

        var image = product.Images.First(i => i.Id == imageId);

        var filePath = image.ImagePath.StartsWith("/uploads/", StringComparison.OrdinalIgnoreCase)
            ? Path.Combine(environment.WebRootPath, image.ImagePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar))
            : null;

        product.Images.Remove(image);
        context.ProductImages.Remove(image);
        await context.SaveChangesAsync();

        var remainingImages = product.Images.Where(i => i.Id != imageId).ToList();
        if (remainingImages.Count > 0 && remainingImages.All(i => !i.IsPrimary))
        {
            remainingImages.OrderBy(i => i.Id).First().IsPrimary = true;
            await context.SaveChangesAsync();
        }

        if (filePath is not null && File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    public async Task AddReviewAsync(int productId, string userId, ReviewInputModel model)
    {
        var review = await context.Reviews.FirstOrDefaultAsync(r => r.ProductId == productId && r.UserId == userId);
        if (review is null)
        {
            await context.Reviews.AddAsync(new Review { ProductId = productId, UserId = userId, Rating = model.Rating, Comment = model.Comment });
        }
        else
        {
            review.Rating = model.Rating;
            review.Comment = model.Comment;
            review.CreatedAt = DateTime.UtcNow;
        }

        await context.SaveChangesAsync();
    }

    private IQueryable<Product> ProductQuery() =>
        context.Products
            .Include(p => p.Category)
            .Include(p => p.Images)
            .Include(p => p.Reviews)
            .ThenInclude(r => r.User);

    private async Task<IReadOnlyList<Product>> GetRecommendedAsync(string? userId)
    {
        if (!string.IsNullOrWhiteSpace(userId))
        {
            var categoryIds = await context.OrderItems
                .Where(i => i.Order != null && i.Order.UserId == userId)
                .Select(i => i.Product!.CategoryId)
                .Distinct()
                .ToListAsync();

            if (categoryIds.Count > 0)
            {
                return await ProductQuery().Where(p => categoryIds.Contains(p.CategoryId)).Take(8).ToListAsync();
            }
        }

        return await ProductQuery()
            .OrderByDescending(p => p.Reviews.Any() ? p.Reviews.Average(r => r.Rating) : 0)
            .ThenByDescending(p => p.OrderItems.Sum(i => i.Quantity))
            .Take(8)
            .ToListAsync();
    }

    private async Task SaveImagesAsync(Product product, IReadOnlyList<IFormFile> images)
    {
        if (images.Count == 0)
        {
            return;
        }

        var uploadRoot = Path.Combine(environment.WebRootPath, "uploads");
        Directory.CreateDirectory(uploadRoot);

        foreach (var image in images.Where(i => i.Length > 0))
        {
            var extension = Path.GetExtension(image.FileName);
            var fileName = $"{Guid.NewGuid():N}{extension}";
            var absolutePath = Path.Combine(uploadRoot, fileName);
            await using var stream = File.Create(absolutePath);
            await image.CopyToAsync(stream);

            product.Images.Add(new ProductImage
            {
                ImagePath = $"/uploads/{fileName}",
                IsPrimary = !product.Images.Any()
            });
        }
    }

    private void DeleteLocalFile(string imagePath)
    {
        var filePath = Path.Combine(environment.WebRootPath, imagePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}
