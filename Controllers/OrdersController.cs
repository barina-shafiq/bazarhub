using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebProject.Interfaces;
using WebProject.Models;

namespace WebProject.Controllers;

[Authorize(Roles = "Customer,Admin")]
public class OrdersController(IOrderService orderService, UserManager<ApplicationUser> userManager) : Controller
{
    public async Task<IActionResult> Index() => View(await orderService.GetUserOrdersAsync(userManager.GetUserId(User)!));

    public async Task<IActionResult> Details(int id)
    {
        var order = await orderService.GetOrderDetailsAsync(id, userManager.GetUserId(User)!);
        return order is null ? NotFound() : View(order);
    }
}
