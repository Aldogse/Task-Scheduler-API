using Microsoft.EntityFrameworkCore;
using TaskScheduler.Contracts;
using TaskSchedulerAPI.Data;
using TaskSchedulerAPI.Interfaces;
using TaskSchedulerAPI.Model;

namespace TaskSchedulerAPI.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDBContext _context;

        public TaskRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public  List<TaskModel> GetAllTasks()
        {
            var tasks =   _context.Tasks.Include(t => t.Status).Include(k => k.TaskLevel).Include(k => k.Owner)
                .ToList();
            return tasks;
        }

        public async Task<TaskModel> GetTaskByIdAsync(int id)
        {
            
            var user =  await _context.Tasks.Where(t => t.Id == id).Include(x => x.Status).Include(x => x.TaskLevel).Include(x => x.Owner).FirstOrDefaultAsync();
            return user;
        }

        public List<TaskModel> GetTaskByUrgency(int task_level_id)
        {
            var tasks = _context.Tasks.Where(l => l.TaskLevelId == task_level_id)
                .Include(t => t.TaskLevel).Include(t => t.Status).Include(x => x.Owner).ToList();

            return tasks;
        }

        public List<TaskModel> GetTasksWithNoOwner()
        {
            List<TaskModel> availableTask = _context.Tasks.Where(t => t.owner_id == null).Include(t => t.Status)
                .Include(k => k.TaskLevel).ToList();
            return availableTask;
        }

        public List<TaskModel> GetUsersTasks(string id)
        {
            List<TaskModel> tasks_for_the_user = _context.Tasks.Where(u => u.owner_id == id)
                                                   .Include(u => u.Owner).Include(u => u.Status).Include(u => u.TaskLevel)
                                                   .ToList();           

            return tasks_for_the_user;
        }

        public TaskLevelModel GetTaskLevel(int id)
        {
            var task_level_name = _context.TaskLevel.Where(t => t.Id == id).FirstOrDefault();
            return task_level_name;
        }

        public async Task<PaginatedResponse<TaskResponse>> PaginatedRequest(int page_size, int page_count)
        {
            IQueryable<TaskModel> all_tasks = _context.Tasks.AsQueryable();

            IQueryable<TaskModel> paginated_query =  _context.Tasks.OrderBy(t => t.Id)
                                                                      .Skip((page_count - 1) * page_size)
                                                                      .Take(page_size);
            List<TaskResponse> paginated_tasks = await paginated_query
                                                .Select(task =>
                                                new TaskResponse(
                                                        task.Id,
                                                        task.TaskName,
                                                        task.Description,
                                                        task.StartDateAndTime.ToString(),
                                                        task.EndDateAndTime.ToString(),
                                                        task.Owner.Email,
                                                        task.Status.Name,
                                                        task.ExpirationDate.ToString(),
                                                        task.TaskLevel.Level
                                                    )).ToListAsync();
            int total_items = await all_tasks.CountAsync();
            return new PaginatedResponse<TaskResponse>(page_size,page_count,total_items,paginated_tasks);
        }
    }
}

   