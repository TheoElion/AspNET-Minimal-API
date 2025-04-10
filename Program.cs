using Microsoft.EntityFrameworkCore;
using RangoAgilApi.DbContexts;
using RangoAgilApi.Extensions;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<RangoDbContext>(
    o => o.UseSqlite(builder.Configuration["ConnectionStrings:RangoDbConStr"])
);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

app.RegisterRangosEndpoints();
app.RegisterIngredientesEndpoints();

app.Run();
