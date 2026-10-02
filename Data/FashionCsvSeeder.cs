using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;
using WebProject.Models;

namespace WebProject.Data;

public static class FashionCsvSeeder
{
    public static string ResolveCsvPath(IHostEnvironment environment, string fileName = "fashion.csv")
    {
        var candidates = new[]
        {
            Path.Combine(environment.ContentRootPath, fileName),
            Path.Combine(AppContext.BaseDirectory, fileName),
            Path.Combine(environment.ContentRootPath, "wwwroot", fileName)
        };

        foreach (var candidate in candidates)
        {
            if (File.Exists(candidate))
            {
                return candidate;
            }
        }

        throw new FileNotFoundException(
            $"Could not find '{fileName}'. Tried: {string.Join(", ", candidates)}");
    }

    public static async Task<int> SeedAsync(IServiceProvider services, string csvPath, int take = 500)
    {
        if (!File.Exists(csvPath))
        {
            throw new FileNotFoundException("Fashion CSV file was not found.", csvPath);
        }

        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var rows = ReadRows(csvPath)
            .Where(r => !string.IsNullOrWhiteSpace(r.ProductTitle))
            .ToList();

        // Use product name as duplicate guard - faster than image URL check
        var existingNames = await context.Products
            .Select(p => p.Name)
            .ToListAsync();

        var existingSet = existingNames.ToHashSet(StringComparer.OrdinalIgnoreCase);
        var random = new Random();
        var selectedRows = rows
            .Where(r => !existingSet.Contains(Trim(r.ProductTitle, 160)))
            .OrderBy(_ => random.Next())
            .Take(take)
            .ToList();

        if (selectedRows.Count == 0)
        {
            return 0;
        }

        var categoryNames = selectedRows
            .Select(r => CleanCategoryName(r.ProductType, r.SubCategory))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        var categories = await context.Categories
            .Where(c => categoryNames.Contains(c.Name))
            .ToListAsync();

        foreach (var categoryName in categoryNames)
        {
            if (categories.All(c => !string.Equals(c.Name, categoryName, StringComparison.OrdinalIgnoreCase)))
            {
                var category = new Category
                {
                    Name = categoryName,
                    Description = $"Fashion products from the {categoryName} collection",
                    IsActive = true
                };
                categories.Add(category);
                await context.Categories.AddAsync(category);
            }
        }

        await context.SaveChangesAsync();

        foreach (var row in selectedRows)
        {
            var categoryName = CleanCategoryName(row.ProductType, row.SubCategory);
            var category = categories.First(c => string.Equals(c.Name, categoryName, StringComparison.OrdinalIgnoreCase));
            var price = RandomPrice(random, row.ProductType, row.SubCategory);
            var hasDiscount = random.NextDouble() < 0.38;
            var discount = hasDiscount ? Math.Round(price * (decimal)(0.82 + random.NextDouble() * 0.1), 0) : (decimal?)null;

            var product = new Product
            {
                Name = Trim(row.ProductTitle, 160),
                Brand = ExtractBrand(row.ProductTitle),
                Description = BuildDescription(row),
                Price = price,
                DiscountPrice = discount,
                StockQuantity = random.Next(5, 151),
                CategoryId = category.Id,
                IsFeatured = random.NextDouble() < 0.16,
                IsTrending = random.NextDouble() < 0.22,
                CreatedAt = DateTime.UtcNow.AddDays(-random.Next(0, 120))
            };

            product.Images.Add(new ProductImage
            {
                ImagePath = !string.IsNullOrWhiteSpace(row.ImageUrl) ? Trim(row.ImageUrl, 300) : "/uploads/sample-fashion.svg",
                IsPrimary = true
            });

            await context.Products.AddAsync(product);
        }

        await context.SaveChangesAsync();
        return selectedRows.Count;
    }

    private static IReadOnlyList<FashionCsvRow> ReadRows(string csvPath)
    {
        using var parser = new TextFieldParser(csvPath);
        parser.SetDelimiters(",");
        parser.HasFieldsEnclosedInQuotes = true;

        var headers = parser.ReadFields() ?? [];
        var headerMap = headers
            .Select((name, index) => new { name, index })
            .ToDictionary(x => x.name, x => x.index, StringComparer.OrdinalIgnoreCase);

        var rows = new List<FashionCsvRow>();
        while (!parser.EndOfData)
        {
            var fields = parser.ReadFields();
            if (fields is null)
            {
                continue;
            }

            rows.Add(new FashionCsvRow(
                Get(fields, headerMap, "ProductId"),
                Get(fields, headerMap, "Gender"),
                Get(fields, headerMap, "Category"),
                Get(fields, headerMap, "SubCategory"),
                Get(fields, headerMap, "ProductType"),
                Get(fields, headerMap, "Colour"),
                Get(fields, headerMap, "Usage"),
                Get(fields, headerMap, "ProductTitle"),
                Get(fields, headerMap, "ImageURL")));
        }

        return rows;
    }

    private static string Get(string[] fields, Dictionary<string, int> headerMap, string key)
    {
        return headerMap.TryGetValue(key, out var index) && index < fields.Length ? fields[index].Trim() : string.Empty;
    }

    private static string CleanCategoryName(string productType, string subCategory)
    {
        var value = !string.IsNullOrWhiteSpace(productType) ? productType : subCategory;
        return Trim(string.IsNullOrWhiteSpace(value) ? "Fashion" : value, 80);
    }

    private static string ExtractBrand(string title)
    {
        var words = title.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (words.Length == 0)
        {
            return "Fashion";
        }

        var stopWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Men", "Women", "Boys", "Girls", "Unisex" };
        var brandWords = words.TakeWhile(w => !stopWords.Contains(w)).Take(3).ToList();
        if (brandWords.Count == 0)
        {
            brandWords = words.Take(2).ToList();
        }

        return Trim(string.Join(' ', brandWords), 120);
    }

    private static string BuildDescription(FashionCsvRow row)
    {
        var parts = new[]
        {
            row.Gender,
            row.Colour,
            row.Usage,
            row.SubCategory,
            row.ProductType
        }.Where(p => !string.IsNullOrWhiteSpace(p));

        return Trim($"{row.ProductTitle}. A {string.Join(", ", parts)} fashion item imported from the catalog CSV.", 900);
    }

    private static decimal RandomPrice(Random random, string productType, string subCategory)
    {
        var key = $"{productType} {subCategory}".ToLowerInvariant();
        var (min, max) = key switch
        {
            var k when k.Contains("shoes") || k.Contains("sandal") => (2500, 14000),
            var k when k.Contains("watch") => (3500, 26000),
            var k when k.Contains("bag") || k.Contains("wallet") => (1800, 16000),
            var k when k.Contains("jacket") || k.Contains("blazer") => (4500, 22000),
            var k when k.Contains("jeans") || k.Contains("trouser") => (2200, 9000),
            var k when k.Contains("dress") || k.Contains("saree") => (3500, 18000),
            _ => (1200, 8500)
        };

        return random.Next(min, max + 1);
    }

    private static string Trim(string value, int maxLength)
    {
        return value.Length <= maxLength ? value : value[..maxLength];
    }

    private sealed record FashionCsvRow(
        string ProductId,
        string Gender,
        string Category,
        string SubCategory,
        string ProductType,
        string Colour,
        string Usage,
        string ProductTitle,
        string ImageUrl);
}
