using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebProject.Interfaces;
using WebProject.Models;

namespace WebProject.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class OrdersController(IOrderService orderService) : Controller
{
    public async Task<IActionResult> Index() => View(await orderService.GetAllOrdersAsync());

    public async Task<IActionResult> Details(int id)
    {
        var order = await orderService.GetOrderDetailsAsync(id);
        return order is null ? NotFound() : View(order);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int id, OrderStatus status)
    {
        await orderService.UpdateStatusAsync(id, status);
        TempData["Success"] = "Order status updated.";
        return RedirectToAction(nameof(Details), new { id });
    }
}
