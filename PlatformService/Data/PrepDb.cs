using System.IO;
using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            using (var servicesScop = app.ApplicationServices.CreateScope())
            {
                SeedData(servicesScop.ServiceProvider.GetService<AppDbContext>(), isProd);
            }
        }

        private static void SeedData(AppDbContext context, bool isProd)
        {
            if (isProd)
            {
                Console.WriteLine("--> Attemping to apply migrations...");
                try
                {
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not run migrations: {ex.Message}");
                }
            }
            
            if (!context.Platforms.Any())
            {
                Console.WriteLine("-->  Seeding Data...");

                context.Platforms.AddRange(
                    new Platform() {Name="Dot Net", Publisher="Microsoft", Cost=0},
                    new Platform() {Name="SQL Server Express", Publisher="Microsoft", Cost=0},
                    new Platform() {Name="Kubernetes", Publisher="Cloud Native Computing Foundation", Cost=0}
                );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("-->  We already have data");
            }
        }
    }
}