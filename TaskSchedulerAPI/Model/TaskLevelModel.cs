namespace TaskSchedulerAPI.Model
{
    public class TaskLevelModel
    {
        public int Id { get; set; }
        public string Level { get; set; }
        public ICollection<TaskModel> TaskModel { get; set; }
    }
}
