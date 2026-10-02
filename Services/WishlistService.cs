using Microsoft.EntityFrameworkCore;
using WebProject.Data;
using WebProject.Interfaces;
using WebProject.Models;

namespace WebProject.Services;

public class WishlistService(ApplicationDbContext context) : IWishlistService
{
    public async Task<IReadOnlyList<WishlistItem>> GetAsync(string userId)
    {
        var wishlist = await GetOrCreateAsync(userId);
        return await context.WishlistItems
            .Include(i => i.Product)!.ThenInclude(p => p!.Images)
            .Where(i => i.WishlistId == wishlist.Id)
            .ToListAsync();
    }

    public async Task ToggleAsync(string userId, int productId)
    {
        var wishlist = await GetOrCreateAsync(userId);
        var item = await context.WishlistItems.FirstOrDefaultAsync(i => i.WishlistId == wishlist.Id && i.ProductId == productId);
        if (item is null)
        {
            await context.WishlistItems.AddAsync(new WishlistItem { WishlistId = wishlist.Id, ProductId = productId });
        }
        else
        {
            context.WishlistItems.Remove(item);
        }

        await context.SaveChangesAsync();
    }

    public async Task RemoveAsync(string userId, int productId)
    {
        var wishlist = await GetOrCreateAsync(userId);
        var item = await context.WishlistItems.FirstOrDefaultAsync(i => i.WishlistId == wishlist.Id && i.ProductId == productId);
        if (item is not null)
        {
            context.WishlistItems.Remove(item);
            await context.SaveChangesAsync();
        }
    }

    private async Task<Wishlist> GetOrCreateAsync(string userId)
    {
        var wishlist = await context.Wishlists.FirstOrDefaultAsync(w => w.UserId == userId);
        if (wishlist is not null)
        {
            return wishlist;
        }

        wishlist = new Wishlist { UserId = userId };
        await context.Wishlists.AddAsync(wishlist);
        await context.SaveChangesAsync();
        return wishlist;
    }
}
