using JsonStreamingServer.Core.Abstractions.Handlers;
using JsonStreamingServer.Core.Abstractions.Services;
using JsonStreamingServer.Core.Abstractions.Suppliers;
using JsonStreamingServer.Core.Handlers;
using JsonStreamingServer.Core.Services;
using JsonStreamingServer.Suppliers.FileStream;
using JsonStreamingServer.Suppliers.FileStream.Services;
using JsonStreamingServer.Suppliers.FileStream.Services.Interfaces;
//using JsonStreamingServer.Suppliers.Generator;
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
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IHotelOffersSupplier, HotelOffersFileStreamSupplier>();
builder.Services.AddSingleton<ICsvHotelReaderService, CsvHotelReaderService>();

builder.Services.AddScoped<HotelOffersRandomErrorHandler>();
builder.Services.AddScoped(sp => ActivatorUtilities.CreateInstance<HotelOfferExternalIdGeneatingHandler>(sp, sp.GetRequiredService<HotelOffersRandomErrorHandler>()));
builder.Services.AddScoped(sp => ActivatorUtilities.CreateInstance<HotelOfferSupplierHandler>(sp, sp.GetRequiredService<HotelOfferExternalIdGeneatingHandler>()));

builder.Services.AddScoped<IHotelOfferRequestHandler>(sp => sp.GetRequiredService<HotelOfferSupplierHandler>()); ;

builder.Services.AddScoped<IHotelService, HotelService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
