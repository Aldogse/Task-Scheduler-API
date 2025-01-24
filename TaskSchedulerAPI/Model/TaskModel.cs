using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskSchedulerAPI.Model
{
    public class TaskModel
    {
        [Key]
        public int Id { get; private set; } 
        public string TaskName { get; private set; }
        public string Description { get; private set; }
        public DateTime StartDateAndTime { get; private set; }
        public DateTime EndDateAndTime { get; private set; }

        [ForeignKey("Owner")]
        public string owner_id { get; private set; }
        public AppUser? Owner { get; private set; }

        [ForeignKey("Status")]
        public int? StatusId { get; private set; }
        public  StatusModel Status { get; private set; }
        public DateTime ExpirationDate { get; private set; }
        [ForeignKey("TaskLevel")]
        public int? TaskLevelId { get; private set; }
        public TaskLevelModel TaskLevel { get; private set; }
        public DateTime LastModifiedDate { get; private set; }

        public TaskModel() { }

        public TaskModel(
                    string taskName,
                    string description,
                    DateTime startDateAndTime,
                    DateTime endDateAndTime,
                    string? owner_Id,
                    int? status_id,
                    DateTime expirationDate,
                    int? taskLevelId,
                    DateTime lastModifiedDate)
        {
            TaskName = taskName;
            Description = description;
            StartDateAndTime = startDateAndTime;
            EndDateAndTime = endDateAndTime;
            owner_id = owner_Id;
            StatusId = status_id;
            ExpirationDate = expirationDate;
            TaskLevelId = taskLevelId;
            LastModifiedDate = lastModifiedDate;           
        }

        public void Update(
                           string taskName,
                           string description,
                           DateTime startDateAndTime,
                           DateTime endDateAndTime,
                           string owner_Id,
                           int? status_id,
                           DateTime expirationDate,
                           int taskLevelId,
                           DateTime lastModifiedDate)
        {            
            TaskName = taskName;
            Description = description;
            StartDateAndTime = startDateAndTime;
            EndDateAndTime = endDateAndTime;
            owner_id = owner_Id;
            StatusId = status_id;
            ExpirationDate = expirationDate;
            TaskLevelId = taskLevelId;
            LastModifiedDate = lastModifiedDate;
        }
        

         
    }
    
}
