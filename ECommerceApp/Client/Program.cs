global using ECommerceApp.Client.Shared;
using Blazored.LocalStorage;
using ECommerceApp.Client;
using ECommerceApp.Client.Services.BasketService;
using ECommerceApp.Client.Services.CategoryService;
using ECommerceApp.Client.Services.ProductService;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped(typeof(IProductService), typeof(ProductService));
builder.Services.AddScoped(typeof(ICategoryService), typeof(CategoryService));
builder.Services.AddScoped(typeof(IBasketService), typeof(BasketService));
//builder.Service.AddScope<IProductService,ProductService>();
await builder.Build().RunAsync();
