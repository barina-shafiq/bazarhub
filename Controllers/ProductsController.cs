using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebProject.Interfaces;
using WebProject.Models;
using WebProject.ViewModels;

namespace WebProject.Controllers;

public class ProductsController(IProductService productService, UserManager<ApplicationUser> userManager) : Controller
{
    public async Task<IActionResult> Index(ProductListViewModel filters) =>
        View(await productService.SearchAsync(filters));

    public async Task<IActionResult> Details(int id)
    {
        var model = await productService.GetDetailsAsync(id);
        return model is null ? NotFound() : View(model);
    }

    [Authorize(Roles = "Customer,Admin")]
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Review(int productId, ReviewInputModel review)
    {
        if (ModelState.IsValid)
        {
            await productService.AddReviewAsync(productId, userManager.GetUserId(User)!, review);
            TempData["Success"] = "Review saved.";
        }

        return RedirectToAction(nameof(Details), new { id = productId });
    }
}
