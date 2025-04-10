using RangoAgilApi.EndpointsHandlers;

namespace RangoAgilApi.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static void RegisterRangosEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var rangosEndpoints = endpointRouteBuilder.MapGroup("/rangos");
        var rangosComIdEndpoints = rangosEndpoints.MapGroup("/{rangoId:int}");

        rangosEndpoints.MapGet("", RangosHandlers.GetRangosAsync);

        rangosComIdEndpoints.MapGet("", RangosHandlers.GetRangoById).WithName("GetRangos");

        rangosEndpoints.MapPost("", RangosHandlers.CreateRangosId);

        rangosComIdEndpoints.MapPut("", RangosHandlers.UpdateRangosAsync);

        rangosComIdEndpoints.MapDelete("", RangosHandlers.DeleteRangosAsync);
    }

    public static void RegisterIngredientesEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var ingredientesEndpoints = endpointRouteBuilder.MapGroup("/rangos/{rangoId:int}/ingredientes");
        ingredientesEndpoints.MapGet("", IngredienteHandlers.GetIngredientesAsync);
        ingredientesEndpoints.MapPost("", () =>
        {
            throw new NotImplementedException();
        });
    }
}
