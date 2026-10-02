using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebProject.Interfaces;
using WebProject.Models;
using WebProject.ViewModels;

namespace WebProject.Controllers;

[Authorize(Roles = "Customer,Admin")]
public class CheckoutController(ICartService cartService, IOrderService orderService, UserManager<ApplicationUser> userManager) : Controller
{
    public async Task<IActionResult> Index()
    {
        var cart = await cartService.GetCartAsync(userManager.GetUserId(User)!);
        if (cart.Items.Count == 0)
        {
            TempData["Error"] = "Your cart is empty.";
            return RedirectToAction("Index", "Cart");
        }

        return View(new CheckoutViewModel { Cart = cart });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(CheckoutViewModel model)
    {
        model.Cart = await cartService.GetCartAsync(userManager.GetUserId(User)!);
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var order = await orderService.CheckoutAsync(userManager.GetUserId(User)!, model);
        return RedirectToAction(nameof(Confirmation), new { id = order.Id });
    }

    public async Task<IActionResult> Confirmation(int id)
    {
        var order = await orderService.GetOrderDetailsAsync(id, userManager.GetUserId(User)!);
        return order is null ? NotFound() : View(order);
    }
}
