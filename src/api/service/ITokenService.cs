namespace api.service;
public interface ITokenService
{
    string GenerateJwtToken(string username, IConfiguration configuration);
    string? GetUsername(HttpContext httpContext);
}