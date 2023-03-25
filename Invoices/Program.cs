
using Invoices.BL.Interface;
using Invoices.BL.Mapper;
using Invoices.BL.Repository;
using Invoices.DAL.Database;
using Invoices.Language;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Products.BL.Repository;
using Sections.BL.Repository;
using System.Globalization;
using Invoices.DAL.Extend;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true)

    .AddNewtonsoftJson(opt => {
        opt.SerializerSettings.ContractResolver = new DefaultContractResolver();
    })

    // Localization Configrations
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
            factory.Create(typeof(SharedResource));
    });

// DbContext Configuration
builder.Services.AddDbContextPool<InvoiceContext>(opt =>
                opt.UseSqlServer(builder.Configuration.GetConnectionString("InvoiceConnection"))
                );
                

// Auto Mapper
builder.Services.AddAutoMapper(x => x.AddProfile(new DomainProfile()));

// Dependency Injection
builder.Services.AddScoped<IInvoiceRep, InvoiceRep>();
builder.Services.AddScoped<ISectionRep, SectionRep>();
builder.Services.AddScoped<IProductRep, ProductRep>();
builder.Services.AddScoped<IInvoiceDetailsRep, InvoiceDetailsRep>();
builder.Services.AddScoped<IInvoiceAttachmentsRep, InvoiceAttachmentsRep>();
builder.Services.AddScoped<IUserRep, UserRep>();
builder.Services.AddScoped<IRolesRep, RolesRep>();


// ------------------------ Identity Configrations  ------------------------------------
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
                options =>
                {
                    options.LoginPath = new PathString("/Home/Index");
                    options.AccessDeniedPath = new PathString("/Account/Login");
                });

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Default Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 0;
}).AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<InvoiceContext>()
    .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>(TokenOptions.DefaultProvider);



// ------------------------ Identity Configrations  ------------------------------------

var app = builder.Build();


// ---------------------- Localization Configrations ----------------------------
var supportedCultures = new[] {
                      new CultureInfo("ar-EG"),
                      new CultureInfo("en-US"),
                };

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures,
    RequestCultureProviders = new List<IRequestCultureProvider>
                {
                new QueryStringRequestCultureProvider(),
                new CookieRequestCultureProvider()
                }
});
// ---------------------- End Localization Configrations ----------------------------

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

// ----- Identity Configrations  ------
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
