using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskSchedulerAPI.Data;
using TaskSchedulerAPI.Interfaces;
using TaskSchedulerAPI.Model;

namespace TaskSchedulerAPI.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDBContext _applicationDBContext;

        public AccountRepository(ApplicationDBContext applicationDBContext)
        {
            _applicationDBContext = applicationDBContext;
        }
        public bool EmailTaken(string email)
        {
            var email_check=  _applicationDBContext.Users.Where(e => e.Email == email).FirstOrDefault();
            if(email_check != null)
            {
                return true;
            }
            return false;         
        }

        public List<AppUser> GetAllUsers()
        {
            return _applicationDBContext.Users.ToList();
        }


        public AppUser GetUser(string id)
        {
            return _applicationDBContext.Users.Where(u => u.Id == id).FirstOrDefault();
        }

        public string GetUserRole(string id)
        {
            //get the user 
            IdentityUserRole<string>?  user = _applicationDBContext.UserRoles.Where(u => u.UserId == id).FirstOrDefault();

            if (user == null)
            {
                return string.Empty;
            }

            IdentityRole? user_role = _applicationDBContext.Roles.Where(u => u.Id == user.RoleId).FirstOrDefault();

            return user_role.Name;
        }
    }
}
