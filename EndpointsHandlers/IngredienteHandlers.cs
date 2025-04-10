using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using RangoAgilApi.DbContexts;
using RangoAgilApi.Models;

namespace RangoAgilApi.EndpointsHandlers;

public static class IngredienteHandlers
{
    public static async Task<Results<NoContent, Ok<IEnumerable<IngredienteDTO>>>> GetIngredientesAsync(RangoDbContext rangoDbContext, IMapper mapper, int rangoId)
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
    }
}
