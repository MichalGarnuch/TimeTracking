using TimeTrackingAPI.Security;
using TimeTrackingAPI.Models; // <- Dodaj to na górze
using Microsoft.EntityFrameworkCore;


namespace TimeTrackingAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.WebHost.UseKestrel(); // Configure binding through ASPNETCORE_URLS.


            builder.Services.AddDbContext<TimeTrackingDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TimeTrackingDBContext")));

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddRequiredBearerAuthentication(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
