using api.dto;

namespace api.service;
interface IUserService
{
    string? ValidateUser(LoginRequestDto request);

}