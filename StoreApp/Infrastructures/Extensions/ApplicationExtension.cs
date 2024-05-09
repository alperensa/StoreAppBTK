using Microsoft.EntityFrameworkCore;
using Repositories;

namespace StoreApp.Infrastructures.Extensions
{
    public static class ApplicationExtension
    {
        public static void ConfigureAndCheck(this IApplicationBuilder app)
        {
            RepositoryContext context = app
            .ApplicationServices
            .CreateScope()
            .ServiceProvider
            .GetRequiredService<RepositoryContext>();

            if(context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
        }

        public static void CongigureLocalization(this WebApplication app)
        {
            app.UseRequestLocalization( options =>
            {
                options.AddSupportedCultures("tr-TR")
                .AddSupportedCultures("tr-TR")
                .SetDefaultCulture("tr-TR");
            });
        }
    }
}