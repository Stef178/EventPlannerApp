using Microsoft.EntityFrameworkCore;
using EventPlannerApp.Data;

namespace EventPlannerApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Voeg de databasecontext toe aan de DI-container met de connection string
            builder.Services.AddDbContext<EventPlannerContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Voeg services toe aan de container
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configureer de HTTP request pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // De standaard HSTS-waarde is 30 dagen. U kunt dit wijzigen voor productieomgevingen.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
