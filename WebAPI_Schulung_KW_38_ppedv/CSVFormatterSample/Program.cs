using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CSVFormatterSample.Data;
using WebApiContrib.Core.Formatter.Csv;

var builder = WebApplication.CreateBuilder(args);


    builder.Services.AddDbContext<CSVFormatterSampleContext>(options =>
    {
        //Mehrzeilig 
        options.UseInMemoryDatabase("MovieDatabase");
    });

// Add services to the container.

builder.Services.AddControllers()
    .AddCsvSerializerFormatters();
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
