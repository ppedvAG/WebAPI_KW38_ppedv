﻿using WebAPIKurs.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebAPIKurs.Data;
using System.Reflection;

namespace WebAPIKurs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //In .NET 6 ist folgendes NEU!!!!
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            

            //builder.WebHost -> IWebHostBuilder -> ASP.NET Core 2.0 / 2.1 

            //builder.Host  -> IHostBuilder -> ASP.NET Core 3.1 / .NET 5.0 

            //AddSingleton, AddScoped, AddTransient -> Lebenszyklen -> Wann wir ein Objekt (Dienst) neu instanziiert. 

            //builder.Services.AddSingleton<...>
            //builder.Services.AddScoped<>
            //builder.Services.AddTransient<> 

            //EF Core 
            //builder.Services.AddScoped... wird intern von -> AddDbContext verwendet 
            //builder.Services.AddDbContext(
            builder.Services.AddSingleton<IDateTimeService, DateTimeService>();


            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            




            #region EFCore Anbindung

            builder.Services.AddDbContext<CarDbContext>(options =>
            {
                //Mehrzeilig 
                options.UseInMemoryDatabase("CarDatabase");
            });

            //options.UseSqlServer(builder.Configuration.GetConnectionString("CarDbContext") ?? throw new InvalidOperationException("Connection string 'CarDbContext' not found.")));
            #endregion

            #region Swagger mit Kommentare
            //!!! Vorab -> Projekteinstellungen -> Build -> Documentation File (aktivieren) 


            //Optionen von SwaggerGen -> Hinzufügen einer Kommentar-Datei


            builder.Services.AddSwaggerGen(options=>
            {
                string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
            });

            #endregion


            WebApplication app = builder.Build();


            using (IServiceScope scope = app.Services.CreateScope())
            {
                CarDbContext context = scope.ServiceProvider.GetRequiredService<CarDbContext>();
                SeedData.Initialize(context);
            }

            




            //Nach builder.Build() k�nnen wir den IOC-Container verwenden.

            //2 Varianten um hier auf den IOC Container zu zugreifen. 

            //Variante 1: 
            IDateTimeService? dateTimeService1 = app.Services.GetService<IDateTimeService>();
            IDateTimeService? dateTimeService2 = app.Services.GetRequiredService<IDateTimeService>();

            //Variante 2: .NET Core 2.1 

            using (IServiceScope scope = app.Services.CreateScope())
            {
                IDateTimeService dateTimeServiceA = scope.ServiceProvider.GetRequiredService<IDateTimeService>();
                IDateTimeService dateTimeServiceB = scope.ServiceProvider.GetService<IDateTimeService>();
            }

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