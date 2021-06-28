using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AllegroAnalitics.Data.Sql.DAO;

namespace AllegroAnalitics.Data.Sql.DAOConfigurations
{
    class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(c => c.ProductId).IsRequired();
            builder.Property(c => c.AllegroProductId).IsRequired();
            builder.Property(c => c.ProductName).IsRequired();
            builder.Property(c => c.BuyNow).IsRequired();
            builder.Property(c => c.RequestId).IsRequired();
           

            builder.HasOne(x => x.Request)
                .WithMany(x => x.Products)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(x => x.RequestId);
        }
    }
}