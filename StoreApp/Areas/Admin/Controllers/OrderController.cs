using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Microsoft.AspNetCore.Authorization;

namespace StoreApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class OrderController : Controller
    {
        private readonly IServiceManager _manager;

        public OrderController(IServiceManager manager)
        {
            _manager = manager;
        }

        public IActionResult Index()
        {
            var orders = _manager.OrderService.Orders;
            return View(orders);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Complete(int id)
        {
            _manager.OrderService.CompleteOrder(id);
            return RedirectToAction("Index");
        }
    }
}