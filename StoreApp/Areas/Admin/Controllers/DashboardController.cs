
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace StoreApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        public IActionResult Index(){
            return View();
        }
    }
}