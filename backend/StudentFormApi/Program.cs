using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StudentFormApi.Data;
using DinkToPdf;
using DinkToPdf.Contracts;
using System.IO;


var builder = WebApplication.CreateBuilder(args);

// Citește connection string-ul din appsettings.json
var connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];

var context = new CustomAssemblyLoadContext();
context.LoadUnmanagedLibrary(Path.Combine(Directory.GetCurrentDirectory(), "Native", "libwkhtmltox.dll"));

builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));


builder.Services.AddDbContext<StudentFormContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Activează Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<StudentFormContext>();
    db.Database.EnsureCreated(); // sau .Migrate() dacă ai folosit EF Migrations
}

