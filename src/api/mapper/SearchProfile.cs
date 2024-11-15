using api.dto;
using api.model;
using AutoMapper;
using Newtonsoft.Json;

namespace api.mapper;

public class SearchProfile : Profile
{
    public SearchProfile()
    {
        CreateMap<SearchDto, RecipeDto>()
            .ForMember(dest => dest.Ingredientes, opt => opt.MapFrom(src => JsonConvert.DeserializeObject<List<string>>(src.Ingredientes ?? "[]")))
            .ForMember(dest => dest.ModoPreparo, opt => opt.MapFrom(src => JsonConvert.DeserializeObject<List<string>>(src.ModoPreparo ?? "[]")));
    }
}