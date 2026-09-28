using AapRepository;
using App.Application;
using App.Application.BulkColumnMapping;
using App.Application.IExternalRepository;
using App.Application.IExternalRepository.QuickBookOnline;
using App.Application.IRepository.Ref_Rep;
using App.Application.IRepository.Sec_Rep;
using App.Application.Services.QuickBooks;
using App.Domain.Entities;
using App.Domain.Entities.QuickBooksOnline;
using App.Domain.Entities.Sec_Model;
using App.Infrastructure;
using App.Infrastructure.ExternalRepository.QBO;
using App.Infrastructure.ExternalRepository.QuickBooksOnline;
using App.Infrastructure.ExternalServices;
using App.Infrastructure.Repository.Ref_Services;
using App.Repository.Repository.Sec_Rep;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DB_Contexts>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register IHttpContextAccessor as singleton for accessing HTTP context
builder.Services.AddHttpContextAccessor();

// Register UnitOfWork as Scoped
builder.Services.AddScoped<Sec_Users>();

builder.Services.AddScoped<QuickBooksToken>();

builder.Services.AddHttpClient<IHttpService, HttpService>(
    (serviceProvider, client) =>
    {
        var settings =serviceProvider.GetRequiredService<IOptions<QBOSettings>>().Value;
        client.BaseAddress =new Uri(settings.BaseUrl);
        client.Timeout =TimeSpan.FromSeconds(60);
    });

builder.Services.Configure<QBOSettings>(
builder.Configuration.GetSection("QuickBooks"));
builder.Services.TryAddScoped<IQuickBooksOnline,QBOService>();
builder.Services.AddScoped<IRefSysDataManagerRep,RefSysDataManagerRep>();
builder.Services.AddScoped<IUserRep,UserRep>();
builder.Services.AddScoped<IQuickBooksTokenRep,QuickBooksTokenRep>();
// Generic Repository
builder.Services.AddScoped(typeof(IRepository<>),typeof(Repository<>));
// Unit Of Work  <-- MISSING
builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
// Sync
builder.Services.AddScoped<SyncDataQuickBooksToken>();
builder.Services.AddScoped<SyncDataQuickBooksCustomer>();
builder.Services.AddScoped<IQuickBooksCustomerRep, QuickBooksCustomerRep>();
builder.Services.AddScoped<IBulkUpsertService, BulkUpsertService>();
builder.Services.AddScoped<SyncDataQuickBooksInvoice>();
builder.Services.AddScoped<IQuickBooksInvoiceRep, QuickBooksInvoiceRep>();


//builder.Services.AddHttpClient<IQuickBooksService, QuickBooksService>();
// Build the app
var app = builder.Build();

// Apply migrations on startup (dev or carefully in prod)
using (var scope = app.Services.CreateScope())
{
    //var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var dbContext = scope.ServiceProvider.GetRequiredService<DB_Contexts>();
    dbContext.Database.Migrate();
}

// Error handling for non-development environment
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
