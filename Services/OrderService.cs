using Microsoft.EntityFrameworkCore;
using WebProject.Data;
using WebProject.Interfaces;
using WebProject.Models;
using WebProject.ViewModels;

namespace WebProject.Services;

public class OrderService(ApplicationDbContext context, ICartService cartService) : IOrderService
{
    public async Task<Order> CheckoutAsync(string userId, CheckoutViewModel model)
    {
        var cart = await cartService.GetCartAsync(userId);
        if (cart.Items.Count == 0)
        {
            throw new InvalidOperationException("Cart is empty.");
        }

        var address = new Address
        {
            UserId = userId,
            FullName = model.FullName,
            Phone = model.Phone,
            City = model.City,
            StreetAddress = model.StreetAddress,
            IsDefault = true
        };

        var order = new Order
        {
            UserId = userId,
            Address = address,
            Subtotal = cart.Subtotal,
            ShippingFee = cart.ShippingFee,
            TotalAmount = cart.Total,
            Status = OrderStatus.Pending,
            Payment = new Payment
            {
                Method = model.PaymentMethod,
                Status = model.PaymentMethod == PaymentMethod.CashOnDelivery ? PaymentStatus.Pending : PaymentStatus.Paid
            }
        };

        foreach (var item in cart.Items)
        {
            if (item.Product is null)
            {
                continue;
            }

            order.Items.Add(new OrderItem
            {
                ProductId = item.ProductId,
                ProductName = item.Product.Name,
                UnitPrice = item.Product.EffectivePrice,
                Quantity = item.Quantity,
                LineTotal = item.Product.EffectivePrice * item.Quantity
            });
            item.Product.StockQuantity = Math.Max(0, item.Product.StockQuantity - item.Quantity);
        }

        await context.Orders.AddAsync(order);
        await context.SaveChangesAsync();
        await cartService.ClearAsync(userId);
        return order;
    }

    public async Task<IReadOnlyList<Order>> GetUserOrdersAsync(string userId) =>
        await context.Orders.Include(o => o.Payment).Where(o => o.UserId == userId).OrderByDescending(o => o.OrderedAt).ToListAsync();

    public async Task<Order?> GetOrderDetailsAsync(int id, string? userId = null)
    {
        var query = context.Orders
            .Include(o => o.User)
            .Include(o => o.Address)
            .Include(o => o.Payment)
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(userId))
        {
            query = query.Where(o => o.UserId == userId);
        }

        return await query.FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<IReadOnlyList<Order>> GetAllOrdersAsync() =>
        await context.Orders.Include(o => o.User).Include(o => o.Payment).OrderByDescending(o => o.OrderedAt).ToListAsync();

    public async Task UpdateStatusAsync(int id, OrderStatus status)
    {
        var order = await context.Orders.FindAsync(id);
        if (order is null)
        {
            return;
        }

        order.Status = status;
        await context.SaveChangesAsync();
    }
}
