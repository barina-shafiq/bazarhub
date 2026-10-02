using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebProject.Data;
using WebProject.Models;

namespace WebProject.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class CategoriesController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index() => View(await context.Categories.Include(c => c.Products).OrderBy(c => c.Name).ToListAsync());

    public IActionResult Create() => View("Edit", new Category());

    public async Task<IActionResult> Edit(int id)
    {
        var category = await context.Categories.FindAsync(id);
        return category is null ? NotFound() : View(category);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Category category)
    {
        if (!ModelState.IsValid)
        {
            return View(category);
        }

        if (category.Id == 0)
        {
            await context.Categories.AddAsync(category);
        }
        else
        {
            context.Categories.Update(category);
        }

        await context.SaveChangesAsync();
        TempData["Success"] = "Category saved.";
        return RedirectToAction(nameof(Index));
    }
}
