using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using AllegroAnalitics.Data.Sql.DAO;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AllegroAnalitics.Data.Sql.DAOConfigurations
{
    class ParametrConfiguration : IEntityTypeConfiguration<Parameter>
    {
        public void Configure(EntityTypeBuilder<Parameter> builder)
        {
            builder.Property(c => c.ParameterId).IsRequired();
            builder.Property(c => c.Name).IsRequired();
            builder.Property(c => c.Value).IsRequired();
            builder.Property(c => c.OrderId).IsRequired();

            builder.HasOne(x => x.Order)
               .WithMany(x => x.Parameters)
               .OnDelete(DeleteBehavior.Cascade)
               .HasForeignKey(x => x.OrderId);
        }
    }
}