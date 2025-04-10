using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RangoAgilApi.DbContexts;
using RangoAgilApi.Entitites;
using RangoAgilApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<RangoDbContext>(
    o => o.UseSqlite(builder.Configuration["ConnectionStrings:RangoDbConStr"])
);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

var rangosEndpoints = app.MapGroup("/rangos");
var rangosComIdEndpoints = rangosEndpoints.MapGroup("/{rangoId:int}");
var ingredientesEndpoints = rangosComIdEndpoints.MapGroup("/ingredientes");

rangosEndpoints.MapGet("", async Task<Results<NoContent, Ok<IEnumerable<RangoDTO>>>> (RangoDbContext rangoDbContext, IMapper mapper, [FromQuery(Name = "name")] string? rangoNome) => {

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

ingredientesEndpoints.MapGet("", async Task<Results<NoContent, Ok<IEnumerable<IngredienteDTO>>>> (RangoDbContext rangoDbContext, IMapper mapper, int rangoId) =>
{
    var rangosEntity = ((await rangoDbContext.Rangos
                               .Include(rango => rango.Ingredientes)
                               .FirstOrDefaultAsync(rango => rango.Id == rangoId))?.Ingredientes);
    if (rangosEntity.Count <= 0 ||rangosEntity == null)
    {
        return TypedResults.NoContent();
    }
    else
    {
        return TypedResults.Ok(mapper.Map<IEnumerable<IngredienteDTO>>(rangosEntity));
    }
});

rangosComIdEndpoints.MapGet("", async Task<Results<NoContent, Ok<RangoDTO>>> (RangoDbContext rangoDbContext, IMapper mapper, int rangoId) => {

    var rangosEntity = await rangoDbContext.Rangos.FirstOrDefaultAsync(x => x.Id == rangoId);

    if (rangosEntity == null)
    {
        return TypedResults.NoContent();
    }
    else
    {
        return TypedResults.Ok(mapper.Map<RangoDTO>(rangosEntity));
    }
}).WithName("GetRangos");

rangosEndpoints.MapPost("", async Task<CreatedAtRoute<RangoDTO>> (RangoDbContext rangoDbContext, IMapper mapper, [FromBody] RangoParaCriacaoDTO  rangoParaCriacaoDTO) =>
{
    var rangoEntity = mapper.Map<Rango>(rangoParaCriacaoDTO);
    rangoDbContext.Add(rangoEntity);
    await rangoDbContext.SaveChangesAsync();

    var rangoToReturn = mapper.Map<RangoDTO>(rangoEntity);

    return TypedResults.CreatedAtRoute(rangoToReturn, "GetRangos", new { rangoId = rangoToReturn.Id });
});

rangosComIdEndpoints.MapPut("", async Task<Results<NotFound, Ok>> (RangoDbContext rangoDbContext, IMapper mapper, [FromBody] RangoParaAtualizacaoDTO  rangoParaAtualizacaoDTO, int rangoId) =>
{
    var rangosEntity = await rangoDbContext.Rangos.FirstOrDefaultAsync(x => x.Id == rangoId);

    if (rangosEntity == null)
    {
        return TypedResults.NotFound();
    }
    mapper.Map(rangoParaAtualizacaoDTO, rangosEntity);

    await rangoDbContext.SaveChangesAsync();

    return TypedResults.Ok();
});

rangosComIdEndpoints.MapDelete("", async Task<Results<NotFound, Ok>> (RangoDbContext rangoDbContext, int rangoId) =>
{
    var rangosEntity = await rangoDbContext.Rangos.FirstOrDefaultAsync(x => x.Id == rangoId);

    if (rangosEntity == null)
    {
        return TypedResults.NotFound();
    }

    rangoDbContext.Rangos.Remove(rangosEntity);

    await rangoDbContext.SaveChangesAsync();

    return TypedResults.Ok();
});

app.Run();
