using ECommerceApp.Core.Repositories;
using ECommerceApp.Core.Service;
using ECommerceApp.Core.UnitOfWorks;
using ECommerceApp.Repository.Data;
using ECommerceApp.Repository.Repositories;
using ECommerceApp.Repository.UnitOfWorks;
using ECommerceApp.Service.Services;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Connection Database
var SqlCon = builder.Configuration.GetConnectionString("SqlCon");
builder.Services.AddDbContext<AppDbContext>(
        options => options.UseMySql(SqlCon,
        ServerVersion.AutoDetect(SqlCon), x =>
        {
            x.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
        })
    );

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
builder.Services.AddScoped(typeof(IProductService), typeof(ProductService));
builder.Services.AddScoped(typeof(ICategoryService), typeof(CategoryService));
builder.Services.AddScoped(typeof(IProductRepository), typeof(ProductRepository));


var app = builder.Build();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSwagger();

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();