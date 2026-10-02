using Microsoft.EntityFrameworkCore;
using WebProject.Data;
using WebProject.Interfaces;
using WebProject.Models;
using WebProject.ViewModels;

namespace WebProject.Services;

public class CartService(ApplicationDbContext context) : ICartService
{
    public async Task<CartViewModel> GetCartAsync(string userId)
    {
        var cart = await GetOrCreateCartAsync(userId);
        await context.Entry(cart).Collection(c => c.Items).Query()
            .Include(i => i.Product)!.ThenInclude(p => p!.Images)
            .LoadAsync();
        return new CartViewModel { Items = cart.Items.ToList() };
    }

    public async Task AddAsync(string userId, int productId, int quantity)
    {
        var cart = await GetOrCreateCartAsync(userId);
        var item = await context.CartItems.FirstOrDefaultAsync(i => i.CartId == cart.Id && i.ProductId == productId);
        if (item is null)
        {
            await context.CartItems.AddAsync(new CartItem { CartId = cart.Id, ProductId = productId, Quantity = Math.Max(1, quantity) });
        }
        else
        {
            item.Quantity += Math.Max(1, quantity);
        }

        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(string userId, int productId, int quantity)
    {
        var cart = await GetOrCreateCartAsync(userId);
        var item = await context.CartItems.FirstOrDefaultAsync(i => i.CartId == cart.Id && i.ProductId == productId);
        if (item is null)
        {
            return;
        }

        if (quantity <= 0)
        {
            context.CartItems.Remove(item);
        }
        else
        {
            item.Quantity = quantity;
        }

        await context.SaveChangesAsync();
    }

    public async Task RemoveAsync(string userId, int productId)
    {
        await UpdateAsync(userId, productId, 0);
    }

    public async Task ClearAsync(string userId)
    {
        var cart = await GetOrCreateCartAsync(userId);
        context.CartItems.RemoveRange(context.CartItems.Where(i => i.CartId == cart.Id));
        await context.SaveChangesAsync();
    }

    private async Task<Cart> GetOrCreateCartAsync(string userId)
    {
        var cart = await context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
        if (cart is not null)
        {
            return cart;
        }

        cart = new Cart { UserId = userId };
        await context.Carts.AddAsync(cart);
        await context.SaveChangesAsync();
        return cart;
    }
}
