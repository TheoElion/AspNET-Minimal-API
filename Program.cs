using Microsoft.EntityFrameworkCore;
using RangoAgilApi.DbContexts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<RangoDbContext>(
    o => o.UseSqlite(builder.Configuration["ConnectionStrings:RangoDbConStr"])
    );

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
