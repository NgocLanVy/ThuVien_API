using Microsoft.AspNetCore.Identity;

namespace WebAPI_simple.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}