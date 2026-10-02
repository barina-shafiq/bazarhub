using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebProject.Interfaces;
using WebProject.Models;

namespace WebProject.Controllers;

[Authorize(Roles = "Customer,Admin")]
public class WishlistController(IWishlistService wishlistService, UserManager<ApplicationUser> userManager) : Controller
{
    public async Task<IActionResult> Index() => View(await wishlistService.GetAsync(userManager.GetUserId(User)!));

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Toggle(int productId)
    {
        await wishlistService.ToggleAsync(userManager.GetUserId(User)!, productId);
        TempData["Success"] = "Wishlist updated.";
        return Redirect(Request.Headers.Referer.ToString() ?? Url.Action(nameof(Index))!);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(int productId)
    {
        await wishlistService.RemoveAsync(userManager.GetUserId(User)!, productId);
        return RedirectToAction(nameof(Index));
    }
}
