using Invoices.BL.Interface;
using Invoices.BL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Invoices.Controllers
{
	[Authorize(Roles = "Admin")]

	public class ProductController : Controller
    {
        #region Fields
        
        private readonly IProductRep product;
        private readonly ISectionRep section;

        #endregion

        #region Ctor

        public ProductController(IProductRep product,ISectionRep section)
        {
            this.product = product;
            this.section = section;
        }

        #endregion
        public IActionResult Index()
        {
            var model = product.Get();
            var sections = section.Get();
            ViewBag.Products = model;
            ViewBag.Sections = sections;
            return View();
        }

        [HttpPost]
        public IActionResult Create(ProductVM model)
        {
            try
            {
                product.Create(model);
                return RedirectToAction("Index");

            }
            catch (Exception)
            {
                TempData["CreateProduct"] = "Faild to Create";
                return RedirectToAction("Index");

            }
        }

        [HttpPost]
        public IActionResult Edit(ProductVM model)
        {
            try
            {
                product.Edit(model);
                return RedirectToAction("Index");

            }
            catch (Exception)
            {
                TempData["Createproduct"] = "Faild to Edit";
                return RedirectToAction("Index");

            }
        }

        public IActionResult Delete(ProductVM model)
        {
            try
            {
                product.Delete(model);
                return RedirectToAction("Index");

            }
            catch (Exception)
            {
                TempData["Createproduct"] = "Faild to Delete";
                return RedirectToAction("Index");

            }
        }

        #region Ajax Requests

        [HttpPost]
        public JsonResult GetProductsBySectionId(int SectionId)
        {
            var data = product.Get(a => a.SectionId == SectionId);
            return Json(data);
        }
        #endregion
    }
}
