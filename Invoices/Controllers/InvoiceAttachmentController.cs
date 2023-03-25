using Invoices.BL.Interface;
using Invoices.BL.Models;
using Invoices.BL.Repository;
using Invoices.DAL.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Invoices.Controllers
{
    [Authorize]

    public class InvoiceAttachmentController : Controller
    {
        private readonly IInvoiceAttachmentsRep attachment;

        public InvoiceAttachmentController(IInvoiceAttachmentsRep attachment)
        {
            this.attachment = attachment;
        }


        [HttpPost]
        public IActionResult Create(InvoiceVM obj)
        {
            try
            {
                attachment.Create(obj);
                return RedirectToAction("Details", "Invoice", new { Id = obj.Id} );

            }
            catch (Exception)
            {
                TempData["AddAttachment"] = "Failed To Add Attachment";
                return RedirectToAction("Details", "Invoice", new { Id = obj.Id });
            }
        }

        public IActionResult Delete(InvoiceAttachmentsVM obj)
        {
            try
            {
                attachment.Delete(obj);
                return RedirectToAction("Details", "Invoice", new { Id = obj.InvoiceId});
            }                                                 
            catch (Exception)                                 
            {                                                 
                TempData["DeleteMsg"] = "Failed To Delete";   
                return RedirectToAction("Details", "Invoice", new { Id = obj.InvoiceId });


            }
        }
    }
}
