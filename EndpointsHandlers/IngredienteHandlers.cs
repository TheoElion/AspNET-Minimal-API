using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RangoAgilApi.DbContexts;
using RangoAgilApi.Models;

namespace RangoAgilApi.EndpointsHandlers;

public static class IngredienteHandlers
{
    public static async Task<Results<NoContent, Ok<IEnumerable<IngredienteDTO>>>> GetIngredientesAsync(RangoDbContext rangoDbContext, IMapper mapper, ILogger<RangoDTO> logger, int rangoId)
    {
        var rangosEntity = ((await rangoDbContext.Rangos
                                   .Include(rango => rango.Ingredientes)
                                   .FirstOrDefaultAsync(rango => rango.Id == rangoId))?.Ingredientes);
        if (rangosEntity.Count <= 0 ||rangosEntity == null)
        {
            logger.LogInformation($"Nenhum rango foi encontrado. Parametro: {rangoId}");
            return TypedResults.NoContent();
        }
        else
        {
            logger.LogInformation("Retornando o rango encontrado.");
            return TypedResults.Ok(mapper.Map<IEnumerable<IngredienteDTO>>(rangosEntity));
        }
    }
}
