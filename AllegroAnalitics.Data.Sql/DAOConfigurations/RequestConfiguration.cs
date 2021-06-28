using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AllegroAnalitics.Data.Sql.DAO;

namespace AllegroAnalitics.Data.Sql.DAOConfigurations
{
    class RequestConfiguration : IEntityTypeConfiguration<Request>
    {
        public void Configure(EntityTypeBuilder<Request> builder)
        {
            builder.Property(c => c.RequestId).IsRequired();
            builder.Property(c => c.Timestamp).IsRequired();
            builder.HasOne(x => x.Order)
               .WithMany(x => x.Requests)
               .OnDelete(DeleteBehavior.Cascade)
               .HasForeignKey(x => x.OrderId);
        }
    }
}