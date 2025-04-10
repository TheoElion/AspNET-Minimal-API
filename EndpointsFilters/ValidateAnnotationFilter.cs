
using MiniValidation;
using RangoAgilApi.Models;

namespace RangoAgilApi.EndpointsFilters;

public class ValidateAnnotationFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var rangoParaCriacaoDTO = context.GetArgument<RangoParaCriacaoDTO>(2);

        if(!MiniValidator.TryValidate(rangoParaCriacaoDTO, out var validationError))
        {
            return TypedResults.ValidationProblem(validationError);

        }

        return await next(context);
    }
}
