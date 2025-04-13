using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StudentFormApi.Data;
using DinkToPdf;
using DinkToPdf.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Citește connection string-ul din appsettings.json
var connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];

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
