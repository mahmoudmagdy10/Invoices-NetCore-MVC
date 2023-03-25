using Azure.Core;
using DocumentFormat.OpenXml.Office2016.Excel;
using Invoices.BL.Interface;
using Invoices.BL.Models;
using Invoices.DAL.Entity;
using Invoices.DAL.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Invoices.Controllers
{
	[Authorize(Roles = "Admin")]

	public class ReportController : Controller
    {
        private readonly IInvoiceRep invoice;

        public ReportController(IInvoiceRep invoice)
        {
            this.invoice = invoice;
        }
        [HttpGet]
        public IActionResult InvoicesReports()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult InvoicesReports(ReportSearchVM model)
        {
            if (model.SearchRadio == 1)
            {
                var Invoices = invoice.InvoicesByDate(model);
                ViewBag.Invoices = Invoices;
                return View();
            }
            else
            {
                var InvoiceNumber = model.InvoiceNumber;
                var Invoices = invoice.Get(a => a.InvoiceNumber == InvoiceNumber);
                ViewBag.Invoices = Invoices;
                return View();

            }
        }

        [HttpGet]
        public IActionResult UsersReports()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UsersReports(ReportSearchVM model)
        {
            var Invoices = invoice.UsersReportsSearch(model);
            ViewBag.Invoices = Invoices;
            return View();

        }

    }
}
