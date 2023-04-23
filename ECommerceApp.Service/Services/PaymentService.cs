using ECommerceApp.Core.Service;
using Microsoft.AspNetCore.Http;
using Stripe;
using Stripe.Apps;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Service.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;
        private readonly IAuthService _authService;

        const string secret = "whsec_968ea49fe37a99abb7f82a10d95d1272766491381eb35a26ab47b0becd8d53d0";

        public PaymentService(IBasketService basketService, IOrderService orderService ,IAuthService authService)
        {
            StripeConfiguration.ApiKey = "sk_test_51N00RgJAB0YQIGyMvjjgzpYCpEhMtGkJwgCpnr390od5ZPmtSrJTPkWiftxFYEHK723BEPTalIqhwFXS9i4Otamv00aDXHUJJm";

            _basketService = basketService;
            _orderService = orderService;
            _authService = authService;
        }

        public async Task<Session> CreateCheckoutSession(int userId,string email)
        {
            var products = (await _basketService.GetDbBasketProducts(userId));
            var lineItems = new List<SessionLineItemOptions>();
            products.ForEach(product => lineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmountDecimal = product.Price * 100,
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = product.Title,
                        Images = new List<string> { product.ImageUrl }
                    }
                },
                Quantity = product.Quantity
            }));

            var options = new SessionCreateOptions
            {
                CustomerEmail = email,
                ShippingAddressCollection =
                    new SessionShippingAddressCollectionOptions
                    {
                        AllowedCountries = new List<string> { "US" }
                    },
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = "https://localhost:7129/order-success",
                CancelUrl = "https://localhost:7129/basket"
            };

            var service = new SessionService();
            Session session = service.Create(options);
            return session;
        }

        public async Task<bool> FulfillOrder(HttpRequest request)
        {
            var json = await new StreamReader(request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                        json,
                        request.Headers["Stripe-Signature"],
                        secret
                    );

                if (stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
                    var session = stripeEvent.Data.Object as Session;
                    var user = await _authService.GetUserByEmail(session.CustomerEmail);
                    await _orderService.PlaceOrder(user.Id);
                }

                return true;
            }
            catch (StripeException e)
            {
                return false;
            }
        }
    }
}

