using Jam_Inspired_Domain;

namespace Jam_Inspired_Application.Interfaces
{
    public interface IJwtGenerator
    {
         string CreateToken(AppUser user);
    }
}