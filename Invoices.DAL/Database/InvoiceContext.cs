using Invoices.DAL.Entity;
using Invoices.DAL.Extend;
using Microsoft.AspNetCore.Identity;
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

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			var hasher = new PasswordHasher<ApplicationUser>();

			// Create a new user for Identity framework
			var user = new ApplicationUser
			{
				UserName = "MahmoudMagdy",
				NormalizedUserName = "MahmoudMagdy".ToUpper(),
				Email = "Mego@gmail.com",
				NormalizedEmail = "Mego@gmail.com".ToUpper(),
				EmailConfirmed = true,
				PasswordHash = hasher.HashPassword(null, "Mego@123"),
				SecurityStamp = string.Empty
			};
			builder.Entity<ApplicationUser>().HasData(user);

			// Create a new role for Identity framework
			var role = new IdentityRole
			{
				Name = "Admin",
				NormalizedName = "ADMIN",
			};
			builder.Entity<IdentityRole>().HasData(role);

			// Assign the role to the user
			builder.Entity<IdentityUserRole<string>>().HasData(
				new IdentityUserRole<string>
				{
					RoleId = role.Id,
					UserId = user.Id,
				});

		}

	}
}
