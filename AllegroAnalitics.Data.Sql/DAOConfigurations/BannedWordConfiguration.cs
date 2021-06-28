using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AllegroAnalitics.Data.Sql.DAO;

namespace AllegroAnalitics.Data.Sql.DAOConfigurations
{
    class BannedWordConfiguration : IEntityTypeConfiguration<BannedWord>
    {
        public void Configure(EntityTypeBuilder<BannedWord> builder)
        {
            builder.Property(c => c.BannedWordId).IsRequired();
            builder.Property(c => c.Word).IsRequired();
            builder.Property(c => c.OrderId).IsRequired();


            builder.HasOne(x => x.Order)
                .WithMany(x => x.BannedWords)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(x => x.OrderId);
        }
    }