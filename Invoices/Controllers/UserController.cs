using Invoices.BL.Interface;
using Invoices.DAL.Extend;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Invoices.BL.Models;
using Microsoft.AspNetCore.Authorization;

namespace Invoices.Controllers
{
	[Authorize(Roles = "Admin")]
	public class UserController : Controller
    {
        #region Fields 

        private readonly IUserRep user;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager;

        #endregion

        #region Ctor 

        public UserController(IUserRep user, UserManager<ApplicationUser> userManager)
        {
            this.user = user;
            this.userManager = userManager;
        }

        #endregion

        #region Actions 

        public IActionResult Index()
        {
            var users = user.Get();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(RegisterVM obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var AddUser = await user.Create(obj);

					if (AddUser != null)
					{
						ViewBag.CreateCompleted = "A New User Created Successfully";
						return RedirectToAction("Index");
					}
                }

                ViewBag.CreateFailed = "Created A New User Is Failed !!";
                return View(obj);
            }
            catch (Exception)
            {
                ViewBag.CreateFailed = "Invalid Data !!";
                return View(obj);
            }
        }
        public async Task<IActionResult> Details(string id)
        {
            var model = await user.GetById(id);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var model = await user.GetById(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ApplicationUser obj)
        {
            try
            {
                await user.Edit(obj);
                if (ModelState.IsValid)
                {
                    //var model =user.Edit(obj);
                    return RedirectToAction("index");
                }
                else
                {
                    TempData["UserUpdate"] = "Failded To Update";
                    return View(obj);
                }

            }
            catch (Exception)
            {
                TempData["UserUpdate"] = "Invalid Data";
                return View(obj);

            }
        }
        public async Task<IActionResult> Delete(ApplicationUser obj)
        {
            try
            {
                await user.Delete(obj);

                if (ModelState.IsValid)
                {
                    return RedirectToAction("index");
                }
                else
                {
                    TempData["UserUpdate"] = "Failded To Delete";
                    return View();
                }

            }
            catch (Exception)
            {
                TempData["UserUpdate"] = "Invalid Data";
                return View();

            }
        }

        #endregion
    }
}
