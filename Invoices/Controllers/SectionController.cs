using Invoices.BL.Interface;
using Invoices.BL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Invoices.Controllers
{
	[Authorize(Roles = "Admin")]

	public class SectionController : Controller
    {
        private readonly ISectionRep section;

        public SectionController(ISectionRep section)
        {
            this.section = section;
        }
        public IActionResult Index()
        {
            var model = section.Get();
            ViewBag.Sections = model;
            return View();
        }

        [HttpPost]
        public IActionResult Create(SectionVM model)
        {
            try
            {
                section.Create(model);
                return RedirectToAction("Index");

            }
            catch (Exception)
            {
                TempData["CreateSection"] = "Faild to Create";
                return RedirectToAction("Index");

            }
        }

        [HttpPost]
        public IActionResult Edit(SectionVM model)
        {
            try
            {
                section.Edit(model);
                return RedirectToAction("Index");

            }
            catch (Exception)
            {
                TempData["CreateSection"] = "Faild to Edit";
                return RedirectToAction("Index");

            }
        }

        public IActionResult Delete(SectionVM model)
        {
            try
            {
                section.Delete(model);
                return RedirectToAction("Index");

            }
            catch (Exception)
            {
                TempData["CreateSection"] = "Faild to Delete";
                return RedirectToAction("Index");

            }
        }
    }
}
