using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebProject.Interfaces;

namespace WebProject.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ReportsController(IReportService reportService) : Controller
{
    public async Task<IActionResult> Index() => View(await reportService.GetDashboardAsync());
}
