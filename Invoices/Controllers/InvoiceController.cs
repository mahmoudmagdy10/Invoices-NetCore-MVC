using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Invoices.BL.Helper;
using Invoices.BL.Interface;
using Invoices.BL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;

namespace Invoices.Controllers
{
	[Authorize(Roles = "Admin,User")]

	public class InvoiceController : Controller
    {
        #region Fields

        private readonly IInvoiceRep invoice;
        private readonly ISectionRep section;
        private readonly IProductRep product;
        private readonly IInvoiceAttachmentsRep invoiceAttachments;
        private readonly IInvoiceDetailsRep invoiceDetails;

        #endregion

        #region Ctor
        public InvoiceController(IInvoiceRep invoice, ISectionRep section, IProductRep product, IInvoiceAttachmentsRep invoiceAttachments, IInvoiceDetailsRep invoiceDetails)
        {
            this.invoice = invoice;
            this.section = section;
            this.product = product;
            this.invoiceAttachments = invoiceAttachments;
            this.invoiceDetails = invoiceDetails;
        }

        #endregion

        #region Actions
        public IActionResult Index()
        {
            var data = invoice.Get();
            ViewBag.Invoices = data;
            return View();
        }
        
        public IActionResult PaiedInvoice()
        {
            return View();
        }
        public IActionResult UnPaiedInvoice()
        {
            return View();
        }
        public IActionResult PartialPaiedInvoice()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            var InvoiceModel = invoice.GetById(id);
            var invoiceAttachmentsModel = invoiceAttachments.Get(a => a.InvoiceId == id);
            var invoiceDetailsModel = invoiceDetails.Get(a => a.InvoiceId == id);

            ViewBag.invoiceAttachmentsModel = invoiceAttachmentsModel;
            ViewBag.invoiceDetailsModel = invoiceDetailsModel;
            return View(InvoiceModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.SectionList = new SelectList(section.Get(), "Id", "Name");
            ViewBag.ProductList = new SelectList(product.Get(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(InvoiceVM obj)
        {
            try
            {
                obj.RestAmount = obj.AmountCollection;
                var Invoice = invoice.Create(obj);
                
                var NewInvoiceDetailsObj = new InvoicesDetailsVM()
                {
                    InvoiceId = Invoice.Id,
                    InvoiceNumber = obj.InvoiceNumber,
                    Notes = obj.Notes,
                    PayDate = obj.PayDate
                };
                
                invoiceDetails.Create(NewInvoiceDetailsObj);
                invoiceAttachments.Create(obj);
                ViewBag.SectionList = new SelectList(section.Get(), "Id", "Name");
                ViewBag.ProductList = new SelectList(product.Get(), "Id", "Name");
                return View();

            }
            catch (Exception)
            {
                return View();

            }
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model = invoice.GetById(id);
            ViewBag.SectionList = new SelectList(section.Get(), "Id", "Name");
            ViewBag.ProductList = new SelectList(product.Get(), "Id", "Name");
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(InvoiceVM obj)
        {
            try
            {
                invoice.Edit(obj);
                return RedirectToAction("index");

            }
            catch (Exception)
            {
                ViewBag.SectionList = new SelectList(section.Get(), "Id", "Name");
                ViewBag.ProductList = new SelectList(product.Get(), "Id", "Name");
                return View();

            }
        }
        public IActionResult Delete(InvoiceVM obj)
        {
            try
            {
                invoice.Delete(obj);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["DeleteMsg"] = "Failed To Delete";
                return RedirectToAction("Index");


            }
        }

        [HttpGet]
        public IActionResult Payment(int id)
        {
            var model = invoice.GetById(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult Payment(InvoiceVM obj)
        {
            var OldInvoice = invoice.GetById(obj.Id);
            try
            {
                if (obj.ValueStatus == 2)
                {
                    var NewInvoiceObj = new InvoiceVM()
                    {
                        Id = obj.Id,
                        Status = "مدفوعة",
                        ValueStatus =2,
                        PayDate = obj.PayDate
                    };

                    var NewInvoiceDetailsObj = new InvoicesDetailsVM()
                    {
                        InvoiceId = obj.Id,
                        InvoiceNumber = obj.InvoiceNumber,
                        Notes = obj.Notes,
                        Status = "مدفوعة",
                        ValueStatus = 2,
                        PayDate = obj.PayDate
                    };

                    invoice.AfterPay(NewInvoiceObj);
                    invoiceDetails.Create(NewInvoiceDetailsObj);
                }

                else if (obj.ValueStatus == 3)
                {

                    var NewAllocatedAmount = OldInvoice.AllocatedAmount + obj.PartialPaiedAmount;

                    if (obj.RestAmount == 0)
                    {
                        var NewInvoiceDetailsObj = new InvoicesDetailsVM()
                        {
                            InvoiceId = obj.Id,
                            InvoiceNumber = obj.InvoiceNumber,
                            Notes = obj.Notes,
                            Status = "مدفوعة",
                            ValueStatus = 2,
                            PayDate = obj.PayDate
                        };
                        var NewInvoiceObj = new InvoiceVM()
                        {
                            Id = obj.Id,
                            Status = "مدفوعة",
                            ValueStatus = 2,
                            PayDate = obj.PayDate,
                        };

                        invoice.AfterPay(NewInvoiceObj);
                        invoiceDetails.Create(NewInvoiceDetailsObj);
                    }
                    else
                    {
                        var NewInvoiceDetailsObj = new InvoicesDetailsVM()
                        {
                            InvoiceId = obj.Id,
                            InvoiceNumber = obj.InvoiceNumber,
                            Notes = obj.Notes,
                            Status = "مدفوع جزئياً",
                            ValueStatus = 3,
                            PayDate = obj.PayDate,
                            AllocatedAmount = NewAllocatedAmount,
                            RestAmount = obj.RestAmount,
                            PartialPaiedAmount = obj.PartialPaiedAmount
                        };

                        var NewInvoiceObj = new InvoiceVM()
                        {
                            Id = obj.Id,
                            Status = "مدفوع جزئياً",
                            ValueStatus = 3,
                            PayDate = obj.PayDate,
                            AllocatedAmount = NewAllocatedAmount,
                            RestAmount = obj.RestAmount,
                            PartialPaiedAmount = obj.PartialPaiedAmount
                        };

                        invoice.AfterPay(NewInvoiceObj);
                        invoiceDetails.Create(NewInvoiceDetailsObj);
                    }
                }

                return RedirectToAction("Details", new { Id = obj.Id });
            }
            catch (Exception)
            {

                TempData["UpdateStatus"] = "Failed To Update Payment Status";
                return RedirectToAction("Details", new { Id = obj.Id });
            }
            #endregion
        }

        public IActionResult PrintInvoice(int id)
        {
            var model = invoice.GetById(id);
            return View(model);
        }

        public IActionResult DownloadFile(int value = 0)
        {
            try
            {
                if (value == 0) 
                {
                    var Invoices = invoice.Get();
                    var stringBuilder = Export.Excel(Invoices);
                    return File(Encoding.Unicode.GetBytes
                    (stringBuilder.ToString()), "text/csv", "Invoices.csv");
                }
                else
                {
                    var Invoices = invoice.Get(a=>a.ValueStatus == value);
                    var stringBuilder = Export.Excel(Invoices);
                    return File(Encoding.Unicode.GetBytes
                    (stringBuilder.ToString()), "text/csv", "Invoices.csv");
                }
            }
            catch
            {
                var data = invoice.Get();
                ViewBag.Invoices = data;

                return RedirectToAction("Index");
            }
        }
    }
}
