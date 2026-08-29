using Entites;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using Services;

namespace CRUD_Operations_Modified
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IPersonService, PersonService>();
            builder.Services.AddTransient<ICountryService, CountryService>();
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DeafultConnection"));
            });
            var app = builder.Build();

            app.UseStaticFiles();
            app.MapControllers();
            app.UseRouting();

            app.Run();
        }
    }
}
