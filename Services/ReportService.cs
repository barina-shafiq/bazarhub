using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebProject.Data;
using WebProject.Interfaces;
using WebProject.Models;
using WebProject.ViewModels;

namespace WebProject.Services;

public class ReportService(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : IReportService
{
    public async Task<DashboardViewModel> GetDashboardAsync()
    {
        var activeOrders = context.Orders.Where(o => o.Status != OrderStatus.Cancelled);
        var monthStart = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1).AddMonths(-5);

        var monthlyRaw = await activeOrders
            .Where(o => o.OrderedAt >= monthStart)
            .GroupBy(o => new { o.OrderedAt.Year, o.OrderedAt.Month })
            .Select(g => new { g.Key.Year, g.Key.Month, Revenue = g.Sum(o => o.TotalAmount) })
            .ToListAsync();

        var monthly = Enumerable.Range(0, 6)
            .Select(offset => monthStart.AddMonths(offset))
            .Select(month =>
            {
                var revenue = monthlyRaw.FirstOrDefault(x => x.Year == month.Year && x.Month == month.Month)?.Revenue ?? 0;
                return new MonthlySalesPoint(month.ToString("MMM yyyy"), revenue);
            })
            .ToList();

        var top = await context.OrderItems
            .GroupBy(i => i.ProductName)
            .OrderByDescending(g => g.Sum(i => i.Quantity))
            .Select(g => new TopProductPoint(g.Key, g.Sum(i => i.Quantity)))
            .Take(5)
            .ToListAsync();

        if (top.Count == 0)
        {
            top = await context.Products
                .OrderByDescending(p => p.IsFeatured)
                .ThenBy(p => p.StockQuantity)
                .Select(p => new TopProductPoint(p.Name, 0))
                .Take(5)
                .ToListAsync();
        }

        var statusRaw = await context.Orders
            .GroupBy(o => o.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToListAsync();

        var statuses = Enum.GetValues<OrderStatus>()
            .Select(status => new StatusPoint(status.ToString(), statusRaw.FirstOrDefault(x => x.Status == status)?.Count ?? 0))
            .ToList();

        var recentOrders = await context.Orders
            .Include(o => o.User)
            .Include(o => o.Payment)
            .OrderByDescending(o => o.OrderedAt)
            .Take(6)
            .ToListAsync();

        var lowStock = await context.Products
            .Include(p => p.Images)
            .OrderBy(p => p.StockQuantity)
            .Take(5)
            .ToListAsync();

        return new DashboardViewModel
        {
            TotalProducts = await context.Products.CountAsync(),
            TotalUsers = await userManager.Users.CountAsync(),
            TotalOrders = await context.Orders.CountAsync(),
            Revenue = await activeOrders.SumAsync(o => (decimal?)o.TotalAmount) ?? 0,
            MonthlySales = monthly,
            TopProducts = top,
            OrdersByStatus = statuses,
            RecentOrders = recentOrders,
            LowStockProducts = lowStock
        };
    }
}
