using Microsoft.AspNetCore.Identity;

namespace TaskSchedulerAPI.Model
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<TaskModel> AssignedTasks { get; set; }
    }
}
