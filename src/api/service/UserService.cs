using api.dto;

namespace api.service;

public class UserService : IUserService
{
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _configuration;

    public UserService(ITokenService tokenService, IConfiguration configuration)
    {
        _tokenService = tokenService;
        _configuration = configuration;
    }

    public string? ValidateUser(LoginRequestDto request)
    {
        // Replace this with actual user validation logic
        if (request.Username == "test" && request.Password == "password")
        {
            return _tokenService.GenerateJwtToken(request.Username, _configuration);
        }
        return null;
    }
}