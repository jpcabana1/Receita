using Newtonsoft.Json;
using api.dto;
using api.model;
using AutoMapper;

namespace api.mapper;

public class RecipeProfile : Profile
{
    public RecipeProfile()
    {
        CreateMap<RecipeDto, Recipe>()
            .ForMember(dest => dest.Ingredientes, opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.Ingredientes)))
            .ForMember(dest => dest.ModoPreparo, opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.ModoPreparo)));

        CreateMap<Recipe, RecipeDto>()
                .ForMember(dest => dest.Ingredientes, opt => opt.MapFrom(src => JsonConvert.DeserializeObject<List<string>>(src.Ingredientes ?? "[]")))
                .ForMember(dest => dest.ModoPreparo, opt => opt.MapFrom(src => JsonConvert.DeserializeObject<List<string>>(src.ModoPreparo ?? "[]")));
    }
}