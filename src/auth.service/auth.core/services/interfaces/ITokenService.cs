using auth.shared.dtos;

namespace auth.core.services.interfaces;

public interface ITokenService
{
    string CreateToken(UserDto user);
}
