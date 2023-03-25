using Invoices.DAL.Entity;
using Invoices.DAL.Extend;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.DAL.Database
{
    public class InvoiceContext : IdentityDbContext<ApplicationUser> 
    {
        public InvoiceContext(DbContextOptions<InvoiceContext> opt) : base(opt)
        {
        }
        public DbSet<Invoice> Invoice { get; set; }
        public DbSet<Section> Section { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<InvoicesDetails> InvoicesDetails { get; set; }
        public DbSet<InvoiceAttachments> InvoiceAttachments { get; set; }

    }
}
