using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using AllegroAnalitics.Data.Sql.DAO;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AllegroAnalitics.Data.Sql.DAOConfigurations
{
    class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(c => c.OrderId).IsRequired();
            builder.Property(c => c.UserId).IsRequired();
        }
    }
}
