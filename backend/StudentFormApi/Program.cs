using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StudentFormApi.Data;
using DinkToPdf;
using DinkToPdf.Contracts;
using System.IO;
using System.Runtime.InteropServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", 
        builder => builder
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod());
            
    options.AddPolicy("AllowAll", 
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];

string wkHtmlToPdfPath = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
    ? "libwkhtmltox.dll"
    : "./Native/libwkhtmltox.so";

CustomAssemblyLoadContext loadContext = new();
loadContext.LoadUnmanagedLibrary("/usr/local/lib/libwkhtmltox.so");

builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

builder.Services.AddDbContext<StudentFormContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


//app.UseCors("AllowReactApp"); 
app.UseCors("AllowAll"); 

//app.UseRouting(); 

app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<StudentFormContext>();
    db.Database.EnsureCreated(); // sau .Migrate() 
}

app.Run();