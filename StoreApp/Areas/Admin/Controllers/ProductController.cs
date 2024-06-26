using Entities.Dtos;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Entities.RequestParameters;
using StoreApp.Models;

namespace StoreApp.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class ProductController : Controller
	{
		private readonly IServiceManager _manager;

		public ProductController(IServiceManager manager)
		{
			_manager = manager;
		}

		public IActionResult Index(ProductRequestParameters p)
		{
			var products = _manager.ProductService.GetAllProductsWithDetails(p);
			var pagination = new Pagination()
			{
				CurrentPage = p.PageNumber,
				ItemsPerPage = p.PageSize,
				TotalItems = _manager.ProductService.GetAllProducts(false).Count()
			};
			return View(new ProductListViewModel()
			{
				Products = products,
				Pagination = pagination
			});
		}
		public IActionResult Create()
		{
			ViewBag.Categories = getCategoriesSelectList();
			
			return View();
		}

		private SelectList getCategoriesSelectList()
		{
			return new SelectList(_manager.CategoryService.GetAllCategories(false),"CategoryId","CategoryName","1");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([FromForm] ProductDtoForInsertion productDto, IFormFile file)
		{
			if (ModelState.IsValid)
			{
				// file operation
			string path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot","images",file.FileName);

			using (var stream = new FileStream(path,FileMode.Create))
			{
				await file.CopyToAsync(stream);

			}
			productDto.ImageUrl = String.Concat("/images/",file.FileName);
			_manager.ProductService.CreateOneProduct(productDto);
			return RedirectToAction("Index");

			}
			return View();
		}
		public IActionResult Update([FromRoute(Name ="id")] int id) 
		{
			ViewBag.Categories = getCategoriesSelectList();
			var model = _manager.ProductService.GetOneProductForUpdate(id, false);
			return View(model); 
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update([FromForm]ProductDtoForUpdate productDto, IFormFile file)
		{
			if(ModelState.IsValid)
			{
				string path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot","images",file.FileName);

			using (var stream = new FileStream(path,FileMode.Create))
			{
				await file.CopyToAsync(stream);

			}
			productDto.ImageUrl = String.Concat("/images/",file.FileName);
			_manager.ProductService.UpdateOneProduct(productDto);
			TempData["success"] = "successfully updated";
			return RedirectToAction("Index");

			}
			return View();
		}
		public IActionResult Delete([FromRoute(Name ="id")] int id)
		{
			
			_manager.ProductService.DeleteOneProduct(id);
			return RedirectToAction("Index");
		}
	}
}
