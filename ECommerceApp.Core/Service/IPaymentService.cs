using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Stripe.Checkout;

namespace ECommerceApp.Core.Service
{
    public interface IPaymentService
    {
        Task<Session> CreateCheckoutSession(int userId,string email);
        Task<bool> FulfillOrder(HttpRequest request);
    }
}
