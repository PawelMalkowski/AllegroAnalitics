using System;
using System.Collections.Generic;
using System.Text;
using AllegroAnalitics.Data.Sql.DAO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AllegroAnalitics.Data.Sql.DAOConfigurations
{

    class OccasionConfiguration : IEntityTypeConfiguration<Occasion>
    {
        public void Configure(EntityTypeBuilder<Occasion> builder)
        {
            builder.Property(c => c.OrderId).IsRequired();
            builder.Property(c => c.ProductId).IsRequired();
            builder.Property(c => c.OccasionId).IsRequired();
        }
    }
}
