using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RangoAgilApi.DbContexts;
using RangoAgilApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<RangoDbContext>(
    o => o.UseSqlite(builder.Configuration["ConnectionStrings:RangoDbConStr"])
);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/rangos", async Task<Results<NoContent, Ok<IEnumerable<RangoDTO>>>> (RangoDbContext rangoDbContext, IMapper mapper, [FromQuery(Name = "name")] string? rangoNome) => {

    var rangosEntity = await rangoDbContext.Rangos
                               .Where(x => rangoNome == null || x.Nome.ToLower().Contains(rangoNome.ToLower()))
                               .ToListAsync();
    if (rangosEntity.Count <= 0 || rangosEntity == null)
    {
        return TypedResults.NoContent();
    }
    else
    {
        return TypedResults.Ok(mapper.Map<IEnumerable<RangoDTO>>(rangosEntity));
    }
});

app.MapGet("/rango/{rangoId:int}/ingredientes", async Task<Results<NoContent, Ok<IEnumerable<IngredienteDTO>>>> (RangoDbContext rangoDbContext, IMapper mapper, int rangoId) =>
{
    var rangosEntity = ((await rangoDbContext.Rangos
                               .Include(rango => rango.Ingredientes)
                               .FirstOrDefaultAsync(rango => rango.Id == rangoId))?.Ingredientes);
    if (rangosEntity == null)
    {
        return TypedResults.NoContent();
    }
    else
    {
        return TypedResults.Ok(mapper.Map<IEnumerable<IngredienteDTO>>(rangosEntity));
    }
});

app.MapGet("/rango/{id:int}", async Task<Results<NoContent, Ok<RangoDTO>>> (RangoDbContext rangoDbContext, IMapper mapper, int id) => {

    var rangosEntity = await rangoDbContext.Rangos.FirstOrDefaultAsync(x => x.Id == id);

    if (rangosEntity == null)
    {
        return TypedResults.NoContent();
    }
    else
    {
        return TypedResults.Ok(mapper.Map<RangoDTO>(rangosEntity));
    }
});

app.Run();
