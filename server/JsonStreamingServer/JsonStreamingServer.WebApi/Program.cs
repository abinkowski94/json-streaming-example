using JsonStreamingServer.Core.Abstractions.Handlers;
using JsonStreamingServer.Core.Abstractions.Services;
using JsonStreamingServer.Core.Abstractions.Suppliers;
using JsonStreamingServer.Core.Handlers;
using JsonStreamingServer.Core.Services;
using JsonStreamingServer.Suppliers.FileStream;
using JsonStreamingServer.Suppliers.FileStream.Services;
using JsonStreamingServer.Suppliers.FileStream.Services.Interfaces;
using JsonStreamingServer.Suppliers.Database;
using JsonStreamingServer.Suppliers.Database.DbContexts;
using JsonStreamingServer.Suppliers.Generator;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(cfg =>
    {
        cfg.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(cfg =>
{
    cfg.MapType<DateOnly>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "date"
    });
});

builder.Services.AddDbContext<HotelOffersDbContext>(optionsBuilder =>
{
    var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DbFiles", "hotels.db");
    optionsBuilder.UseSqlite($"Data Source={dbPath}");
});

builder.Services.AddSingleton<IHotelOffersSupplier, HotelOffersFileStreamSupplier>();
builder.Services.AddSingleton<ICsvHotelReaderService, CsvHotelReaderService>();
builder.Services.AddScoped<IHotelOffersSupplier, DatabaseHotelOffersSupplier>();
builder.Services.AddSingleton<IHotelOffersSupplier, HotelOffersSupplierGenerator>();

builder.Services.AddScoped<HotelOffersMaxResultsHandler>();

builder.Services.AddScoped(sp => ActivatorUtilities
    .CreateInstance<HotelOffersRandomErrorHandler>(sp, sp.GetRequiredService<HotelOffersMaxResultsHandler>()));

builder.Services.AddScoped(sp => ActivatorUtilities
    .CreateInstance<HotelOfferExternalIdGeneatingHandler>(sp, sp.GetRequiredService<HotelOffersRandomErrorHandler>()));

builder.Services.AddScoped(sp => ActivatorUtilities
    .CreateInstance<HotelOfferSupplierHandler>(sp, sp.GetRequiredService<HotelOfferExternalIdGeneatingHandler>()));

builder.Services.AddScoped<IHotelOfferRequestHandler>(sp => sp.GetRequiredService<HotelOfferSupplierHandler>());
builder.Services.AddScoped<IHotelService, HotelService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors(builder => builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
