using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AllegroAnalitics.Data.Sql.DAO;

namespace AllegroAnalitics.Data.Sql.DAOConfigurations
{
    class CategoryConfiguration: IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(c => c.CategoryId).IsRequired();
            builder.Property(c => c.CategoryAllegroId).IsRequired();
            builder.Property(c => c.OrderId).IsRequired();


            builder.HasOne(x => x.Order)
                .WithMany(x => x.Categories)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(x => x.OrderId);
        }
    }
}
