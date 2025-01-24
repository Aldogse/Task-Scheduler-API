namespace TaskSchedulerAPI.Model
{
    public class StatusModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<TaskModel> TaskModel { get; set; }
    }
}
