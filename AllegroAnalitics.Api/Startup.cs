using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AllegroAnalitics.Data.Sql;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using AllegroAnalitics.Data.Identity;
using AllegroAnalitics.IServices.User;
using AllegroAnalitics.Services.User;
using AllegroAnalitics.Api.Middlewares;
using AllegroAnalitics.Api.Validation;
using FluentValidation;
using AllegroAnalitics.IServices.Data;
using AllegroAnalitics.Services.Data;
using AllegroAnalitics.IData.Data;
using AllegroAnalitics.Data.Sql.Data;
using AllegroAnalitics.DataAgregation;
using Newtonsoft.Json.Serialization;

namespace AllegroAnalitics.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
       
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                                  builder =>
                                  {
                                      builder.WithOrigins("http://localhost", "null", "http://teniswebsite.example.com:3000");
                                      builder.AllowAnyMethod();
                                      builder.AllowAnyHeader();
                                      builder.AllowCredentials();

                                  });
            });
            services.AddControllers();
            services.AddDbContextPool <AllegroAnaliticsIdentityContext>(options => options.UseMySql(Configuration.GetConnectionString("IdentityDbContext"), ServerVersion.AutoDetect(Configuration.GetConnectionString("IdentityDbContext"))));
            services.AddDbContext<AllegroAnaliticsDbContext>(options => options.UseMySql(Configuration.GetConnectionString("AllegroAnaliticsDbContext"), ServerVersion.AutoDetect(Configuration.GetConnectionString("AllegroAnaliticsDbContext"))));
            services.AddTransient<Data.Identity.Migrations.DatabaseSeed>();
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 2;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.User.AllowedUserNameCharacters = "aπbcÊdeÍfghijkl≥mnÒoÛpqrsútuvwxyzüøA•BC∆DE FGHIJKL£MN—O”PRSåTUWYZèØ1234567890-_$.";
            })
          .AddEntityFrameworkStores<AllegroAnaliticsIdentityContext>().AddDefaultTokenProviders();

            services.AddRouting();
            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.UseApiBehavior = false;
            });
           
            services.AddScoped<IValidator<IServices.Request.CreatUser>, CreateUserValidator>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDataService, DataService>();
            services.AddScoped<IDataRepository, DataRepository>();
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
         /*   using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {

                var context = serviceScope.ServiceProvider.GetRequiredService<AllegroAnaliticsIdentityContext>();
                var databaseSeed = serviceScope.ServiceProvider.GetRequiredService<Data.Identity.Migrations.DatabaseSeed>();
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                var a = databaseSeed.Seed();
                a.Wait();
            }
           
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<AllegroAnaliticsDbContext>();
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
        */   
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
           

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapDefaultControllerRoute();
            });
            app.UseApiVersioning();
            app.UseMiddleware<ErrorHandlerMiddleware>();
            RequestController requestController = new RequestController();
        }
    }
}
