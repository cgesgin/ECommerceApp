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
    public class ProductTypeSeed : IEntityTypeConfiguration<ProductType>
    {
        public void Configure(EntityTypeBuilder<ProductType> builder)
        {
            builder.HasData(
                    new ProductType
                    {
                        Id = 1,
                        Name = "Default"
                    },
                    new ProductType
                    {
                        Id = 2,
                        Name = "E-Book"
                    },
                    new ProductType
                    {
                        Id = 3,
                        Name = "AudioBook"
                    }
                );
        }
    }
}
