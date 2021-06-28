using Microsoft.EntityFrameworkCore;
using AllegroAnalitics.Data.Sql.DAO;
using AllegroAnalitics.Data.Sql.DAOConfigurations;
using System;
using System.Collections.Generic;
using System.Text;

namespace AllegroAnalitics.Data.Sql
{
    public class AllegroAnaliticsDbContext : DbContext
    {
        public AllegroAnaliticsDbContext(DbContextOptions<AllegroAnaliticsDbContext> options) : base(options) { }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<Parameter> Parameter { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Request> Request { get; set; }
        public virtual DbSet<BannedWord> BannedWord {get; set;}
        public virtual DbSet<NecessaryWord> NecessaryWord { get; set; }
        public virtual DbSet<Occasion> Occasions { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new CategoryConfiguration());
            builder.ApplyConfiguration(new OrderConfiguration());
            builder.ApplyConfiguration(new ParametrConfiguration());
            builder.ApplyConfiguration(new ProductConfiguration());
            builder.ApplyConfiguration(new RequestConfiguration());
            builder.ApplyConfiguration(new BannedWordConfiguration());
            builder.ApplyConfiguration(new NecessaryWordConfiguration());
            builder.ApplyConfiguration(new OccasionConfiguration());

        }
    }
}
