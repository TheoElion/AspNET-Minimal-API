using AutoMapper;
using RangoAgilApi.Entitites;
using RangoAgilApi.Models;

namespace RangoAgilApi.Profiles;

public class RangoAgilProfile : Profile
{

    public RangoAgilProfile()
    {
        CreateMap<Rango, RangoDTO>().ReverseMap();
        CreateMap<Ingrediente, IngredienteDTO>()
            .ForMember(
                d => d.RangoId,
                o => o.MapFrom(s => s.Rangos.First().Id)
            );
    }
}

