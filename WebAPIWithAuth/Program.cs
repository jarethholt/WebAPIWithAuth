using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WebAPIWithAuth.Data;
using WebAPIWithAuth.Helpers;
using WebAPIWithAuth.Models;
using WebAPIWithAuth.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<HeroContext>(
    db => db.UseSqlServer(
        builder.Configuration.GetConnectionString("HeroConnectionString")),
    ServiceLifetime.Singleton);

builder.Services.AddSingleton<IHeroService, HeroService>();

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddScoped<IUserService, UserService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(opts =>
{
    // Generate the default UI of Swagger documentation
    opts.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "JWT Token Authentication API",
        Description = ".NET 8 Web API"
    });

    // Enable authorization using Swagger
    opts.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = """
        JWT authorization header using the Bearer scheme.
        
        Enter 'Bearer' [space] and then your token in the text input below.

        Example: "Bearer 12345abcdef"
        """
    });

    opts.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

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

app.UseMiddleware<JwtMiddleware>();

app.Run();
