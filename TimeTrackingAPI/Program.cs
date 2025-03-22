using TimeTrackingAPI.Models; // <- Dodaj to na górze
using Microsoft.EntityFrameworkCore;


namespace TimeTrackingAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.WebHost.UseKestrel().UseUrls("http://0.0.0.0:5215");


            builder.Services.AddDbContext<TimeTrackingDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TimeTrackingDBContext")));

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
