using System.Text.Json;
using PriceCalculator.Api.Bll.Services;
using PriceCalculator.Api.Bll.Services.Interfaces;
using PriceCalculator.Api.Dal.Repositories;
using PriceCalculator.Api.Dal.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.CustomSchemaIds(x => x.FullName);
});

builder.Services.AddScoped<IPriceCalculatorService, PriceCalculatorService>();
builder.Services.AddSingleton<IStorageRepository, StorageRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapControllers();
app.UseAuthorization();
app.UseHttpsRedirection();

app.Run();

