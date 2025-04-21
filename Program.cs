using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RangoAgilApi.DbContexts;
using RangoAgilApi.Extensions;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<RangoDbContext>(
    o => o.UseSqlite(builder.Configuration["ConnectionStrings:RangoDbConStr"])
);

builder.Services.AddIdentityApiEndpoints<IdentityUser>()
        .AddEntityFrameworkStores<RangoDbContext>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddProblemDetails();

builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();

builder.Services.AddAuthorizationBuilder();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("TokenAuthRango",
        new()
        {
            Name = "Authorization",
            Description = "Token baseado em Autenticação e Autorização",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            In = ParameterLocation.Header
        }
    );
    options.AddSecurityRequirement(new()
        {
            {
                new()
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "TokenAuthRango"
                    }
                },
                new List<string>()
            }
        }
    );
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorDev",
        policy => policy
            .WithOrigins("https://localhost:7247")
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

app.UseCors("AllowBlazorDev");

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
}

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.RegisterRangosEndpoints();
app.RegisterIngredientesEndpoints();

app.Run();
