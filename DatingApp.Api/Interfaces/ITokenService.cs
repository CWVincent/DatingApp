using DatingApp.Api.Entities;
using System.Threading.Tasks;

namespace DatingApp.Api.Interfaces
{
    public interface ITokenService
    {
        //
        Task<string> CreateToken(AppUser user);
    }
}
