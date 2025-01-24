using Microsoft.AspNetCore.Identity;
using TaskSchedulerAPI.Model;

namespace TaskSchedulerAPI.Interfaces
{
    public interface IAccountRepository
    {
        bool EmailTaken(string email);
        List<AppUser>GetAllUsers();
        AppUser GetUser(string id);
        string GetUserRole(string id);
    }
}
