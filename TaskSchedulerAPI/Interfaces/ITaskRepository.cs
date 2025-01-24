using TaskScheduler.Contracts;
using TaskSchedulerAPI.Model;

namespace TaskSchedulerAPI.Interfaces
{
    public interface ITaskRepository
    {
        List<TaskModel> GetAllTasks();
        Task<TaskModel> GetTaskByIdAsync(int id);
        List<TaskModel> GetTasksWithNoOwner();
        List<TaskModel> GetTaskByUrgency(int task_level_id);
        List<TaskModel> GetUsersTasks(string id);
        TaskLevelModel GetTaskLevel(int id);
        Task<PaginatedResponse<TaskResponse>> PaginatedRequest(int page_size,int page_count);
    }
}
