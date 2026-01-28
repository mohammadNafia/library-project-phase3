using MiniLibrary.Domain.Entities;

namespace MiniLibrary.Application.Services;

public interface IJwtTokenService
{
    string GenerateToken(User user);
}