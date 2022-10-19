using ECommerceApp.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Repository.Seed
{
    public class ProductVariantSeed : IEntityTypeConfiguration<ProductVariant>
    {
        public void Configure(EntityTypeBuilder<ProductVariant> builder)
        {
            builder.HasData(
                    new ProductVariant
                    {
                        ProductId=1,
                        ProductTypeId=2,
                        Price=9.99m,
                        OriginalPrice=19.99m
                    },
                     new ProductVariant
                     {
                         ProductId = 1,
                         ProductTypeId = 3,
                         Price = 9.99m,
                         OriginalPrice = 15.99m
                     },
                     new ProductVariant
                     {
                         ProductId = 2,
                         ProductTypeId = 3,
                         Price = 9.99m,
                     }
                );
        }
    }
}
