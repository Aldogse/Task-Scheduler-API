using TaskSchedulerAPI.Model;

namespace TaskSchedulerAPI.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
        string RefreshToken();
    }
}
