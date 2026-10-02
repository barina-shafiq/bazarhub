using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebProject.Interfaces;
using WebProject.Models;

namespace WebProject.Controllers;

[Authorize(Roles = "Customer,Admin")]
public class CartController(ICartService cartService, UserManager<ApplicationUser> userManager) : Controller
{
    public async Task<IActionResult> Index() => View(await cartService.GetCartAsync(userManager.GetUserId(User)!));

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(int productId, int quantity = 1)
    {
        await cartService.AddAsync(userManager.GetUserId(User)!, productId, quantity);
        TempData["Success"] = "Product added to cart.";
        return Redirect(Request.Headers.Referer.ToString() ?? Url.Action(nameof(Index))!);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int productId, int quantity)
    {
        await cartService.UpdateAsync(userManager.GetUserId(User)!, productId, quantity);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(int productId)
    {
        await cartService.RemoveAsync(userManager.GetUserId(User)!, productId);
        return RedirectToAction(nameof(Index));
    }
}
