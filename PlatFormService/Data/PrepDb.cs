using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PlatFormService.Models;

namespace PlatFormService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
            }

        }
        private static void SeedData(AppDbContext context, bool isProd)
        {
            if (isProd)
            {
                Console.WriteLine("--> Attempting to aplly migrations");

                try
                {
                    context.Database.Migrate();

                }
                catch (System.Exception ex)
                {

                    Console.WriteLine($"--> Could not run  migrations:{ex.Message}");

                }
            }

            if (!context.Platforms.Any())
            {
                Console.WriteLine("--> Seeding Data");
                context.Platforms.AddRange(
                    new Platform() { Name = "Dotnet", Publisher = "Microsoft", Cost = "Free" },
                    new Platform() { Name = "Sql Server", Publisher = "Microsoft", Cost = "Free" },
                    new Platform() { Name = "Kubernetes", Publisher = "Cloud Native", Cost = "Free" }


                );
                context.SaveChanges();

            }
            else
            {
                Console.WriteLine("--> We already have data");
            }

        }

    }
}