using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RangoAgilApi.DbContexts;
using RangoAgilApi.Entitites;
using RangoAgilApi.Models;

namespace RangoAgilApi.EndpointsHandlers;

public static class RangosHandlers
{
    public static async Task<Results<NoContent, Ok<IEnumerable<RangoDTO>>>> GetRangosAsync(RangoDbContext rangoDbContext, IMapper mapper, [FromQuery(Name = "name")] string? rangoNome)
    {
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
    }

    public static async Task<Results<NoContent, Ok<RangoDTO>>> GetRangoById(RangoDbContext rangoDbContext, IMapper mapper, int rangoId)
    {
        var rangosEntity = await rangoDbContext.Rangos.FirstOrDefaultAsync(x => x.Id == rangoId);

        if (rangosEntity == null)
        {
            return TypedResults.NoContent();
        }
        else
        {
            return TypedResults.Ok(mapper.Map<RangoDTO>(rangosEntity));
        }
    }

    public static async Task<CreatedAtRoute<RangoDTO>> CreateRangosId(RangoDbContext rangoDbContext, IMapper mapper, [FromBody] RangoParaCriacaoDTO  rangoParaCriacaoDTO)
    {
        var rangoEntity = mapper.Map<Rango>(rangoParaCriacaoDTO);
        rangoDbContext.Add(rangoEntity);
        await rangoDbContext.SaveChangesAsync();

        var rangoToReturn = mapper.Map<RangoDTO>(rangoEntity);

        return TypedResults.CreatedAtRoute(rangoToReturn, "GetRangos", new { rangoId = rangoToReturn.Id});
    }

    public static async Task<Results<NotFound, Ok>> UpdateRangosAsync(RangoDbContext rangoDbContext, IMapper mapper, [FromBody] RangoParaAtualizacaoDTO  rangoParaAtualizacaoDTO, int rangoId)
    {
        var rangosEntity = await rangoDbContext.Rangos.FirstOrDefaultAsync(x => x.Id == rangoId);

        if (rangosEntity == null)
        {
            return TypedResults.NotFound();
        }
        mapper.Map(rangoParaAtualizacaoDTO, rangosEntity);

        await rangoDbContext.SaveChangesAsync();

        return TypedResults.Ok();
    }

    public static async Task<Results<NotFound, Ok>> DeleteRangosAsync(RangoDbContext rangoDbContext, int rangoId)
    {
        var rangosEntity = await rangoDbContext.Rangos.FirstOrDefaultAsync(x => x.Id == rangoId);

        if (rangosEntity == null)
        {
            return TypedResults.NotFound();
        }

        rangoDbContext.Rangos.Remove(rangosEntity);

        await rangoDbContext.SaveChangesAsync();

        return TypedResults.Ok();
    }
}
    
