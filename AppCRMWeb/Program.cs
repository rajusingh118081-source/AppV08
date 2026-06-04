using AapRepository;
using App.Application.IExternalRepository;
using App.Domain.Entities;
using App.Domain.Entities.Sec_Model;
using App.Infrastructure.ExternalRepository.QBO;
using App.Infrastructure.ExternalRepository.QuickBooksOnline;
using App.Infrastructure.ExternalServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DB_Contexts>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register IHttpContextAccessor as singleton for accessing HTTP context
builder.Services.AddHttpContextAccessor();

// Register UnitOfWork as Scoped
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<Sec_Users>();
builder.Services.AddHttpClient<IHttpService, HttpService>();
builder.Services.Configure<QBOSettings>(builder.Configuration.GetSection("QuickBooks"));
builder.Services.TryAddScoped<IQuickBooksOnline, QBOService>();
builder.Services.AddHttpClient<IQuickBooksService, QuickBooksService>();
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
