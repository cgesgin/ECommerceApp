global using ECommerceApp.Client.Shared;
using Blazored.LocalStorage;
using ECommerceApp.Client;
using ECommerceApp.Client.Services.AddressService;
using ECommerceApp.Client.Services.AuthService;
using ECommerceApp.Client.Services.BasketService;
using ECommerceApp.Client.Services.CategoryService;
using ECommerceApp.Client.Services.OrderService;
using ECommerceApp.Client.Services.ProductService;
using ECommerceApp.Client.Services.ProductTypeService;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddMudServices();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped(typeof(IProductService), typeof(ProductService));
builder.Services.AddScoped(typeof(ICategoryService), typeof(CategoryService));
builder.Services.AddScoped(typeof(IBasketService), typeof(BasketService));
builder.Services.AddScoped(typeof(IAuthService), typeof(AuthService));
builder.Services.AddScoped(typeof(IOrderService), typeof(OrderService));
builder.Services.AddScoped(typeof(IAddressService), typeof(AddressService));
builder.Services.AddScoped(typeof(IProductTypeService), typeof(ProductTypeService));

builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped(typeof(AuthenticationStateProvider), typeof(CostumAuthStateProvider));

//builder.Service.AddScope<IProductService,ProductService>();
await builder.Build().RunAsync();
