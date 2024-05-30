using Microsoft.EntityFrameworkCore;
using WebAPIWithAuth.AuthHelpers;
using WebAPIWithAuth.Data;
using WebAPIWithAuth.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<HeroContext>(
    db => db.UseSqlServer(
        builder.Configuration.GetConnectionString("HeroConnectionString")),
    ServiceLifetime.Singleton);

builder.Services.AddSingleton<IHeroService, HeroService>();

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

app.UseApiKey();
app.UseAuthorization();

app.MapControllers();

app.Run();
