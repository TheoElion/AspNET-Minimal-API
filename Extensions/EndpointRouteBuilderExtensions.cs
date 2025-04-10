using RangoAgilApi.EndpointsFilters;
using RangoAgilApi.EndpointsHandlers;

namespace RangoAgilApi.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static void RegisterRangosEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var rangosEndpoints = endpointRouteBuilder.MapGroup("/rangos");
        var rangosComIdEndpoints = rangosEndpoints.MapGroup("/{rangoId:int}");
        var rangosComIdAndLockFilterEndpoints = endpointRouteBuilder.MapGroup("/rangos/{rangoId:int}")
                .AddEndpointFilter(new RangoIsLockedFilter(4))
                .AddEndpointFilter(new RangoIsLockedFilter(10));

        rangosEndpoints.MapGet("", RangosHandlers.GetRangosAsync);

        rangosComIdEndpoints.MapGet("", RangosHandlers.GetRangoById).WithName("GetRangos");

        rangosEndpoints.MapPost("", RangosHandlers.CreateRangosAsync)
                .AddEndpointFilter<ValidateAnnotationFilter>();

        rangosComIdAndLockFilterEndpoints.MapPut("", RangosHandlers.UpdateRangosAsync);

        rangosComIdAndLockFilterEndpoints.MapDelete("", RangosHandlers.DeleteRangosAsync)
                .AddEndpointFilter<LogNotFoundResponseFilter>();
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
