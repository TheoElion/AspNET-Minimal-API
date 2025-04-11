using Microsoft.AspNetCore.Identity;
using RangoAgilApi.EndpointsFilters;
using RangoAgilApi.EndpointsHandlers;

namespace RangoAgilApi.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static void RegisterRangosEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGroup("/identity/").MapIdentityApi<IdentityUser>();

        var rangosEndpoints = endpointRouteBuilder.MapGroup("/rangos").RequireAuthorization();
        var rangosComIdEndpoints = rangosEndpoints.MapGroup("/{rangoId:int}");

        var rangosComIdAndLockFilterEndpoints = endpointRouteBuilder.MapGroup("/rangos/{rangoId:int}")
                .RequireAuthorization("RequireAdminFromBrazil")
                .RequireAuthorization()
                .AddEndpointFilter(new RangoIsLockedFilter(4))
                .AddEndpointFilter(new RangoIsLockedFilter(10));

        rangosEndpoints.MapGet("", RangosHandlers.GetRangosAsync);

        rangosComIdEndpoints.MapGet("", RangosHandlers.GetRangoById).WithName("GetRangos")
                .AllowAnonymous();

        rangosEndpoints.MapPost("", RangosHandlers.CreateRangosAsync)
                .AddEndpointFilter<ValidateAnnotationFilter>();

        rangosComIdAndLockFilterEndpoints.MapPut("", RangosHandlers.UpdateRangosAsync);

        rangosComIdAndLockFilterEndpoints.MapDelete("", RangosHandlers.DeleteRangosAsync)
                .AddEndpointFilter<LogNotFoundResponseFilter>();
    }

    public static void RegisterIngredientesEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var ingredientesEndpoints = endpointRouteBuilder.MapGroup("/rangos/{rangoId:int}/ingredientes")
                .RequireAuthorization();
        ingredientesEndpoints.MapGet("", IngredienteHandlers.GetIngredientesAsync);
    }
}
