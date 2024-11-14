using api.dto;
using api.model;
using AutoMapper;

namespace api.service;

public class RecipeService : IRecipeService
{
    private readonly IMapper _mapper;
    private readonly RecipeContext _context;

    public RecipeService(RecipeContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public void AddRecipes(List<RecipeDto> recipes)
    {
        _context.Recipes.AddRange(recipes.Select(_mapper.Map<Recipe>));
        _context.SaveChanges();
    }
}