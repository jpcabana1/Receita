using api.dto;
using api.model;
using Microsoft.EntityFrameworkCore;

namespace api.service;

public class UserService : IUserService
{
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _configuration;
    private readonly RecipeContext _context;
    public UserService(ITokenService tokenService
        , IConfiguration configuration
        , RecipeContext context)
    {
        _tokenService = tokenService;
        _configuration = configuration;
        _context = context;
    }

    public string? ValidateUser(LoginRequestDto request)
    {
        if (_context.Users.Any(fd => fd.Username == request.Username && fd.Password == request.Password))
        {
            return _tokenService.GenerateJwtToken(request.Username!, _configuration);
        }
        return null;
    }
}