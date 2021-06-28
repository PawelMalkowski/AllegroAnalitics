using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AllegroAnalitics.Data.Sql.DAO;

namespace AllegroAnalitics.Data.Sql.DAOConfigurations
{
    class NecessaryWordConfiguration : IEntityTypeConfiguration<NecessaryWord>
    {
        public void Configure(EntityTypeBuilder<NecessaryWord> builder)
        {
            builder.Property(c => c.NecessaryWordId).IsRequired();
            builder.Property(c => c.Word).IsRequired();
            builder.Property(c => c.OrderId).IsRequired();


            builder.HasOne(x => x.Order)
                .WithMany(x => x.NecessaryWords)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(x => x.OrderId);
        }
    }
}
