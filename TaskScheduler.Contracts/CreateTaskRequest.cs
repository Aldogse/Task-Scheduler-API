using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TaskScheduler.Contracts
{
  public record CreateTaskRequest(
      string TaskName,
      string Description,
      string StartDate,
      string StartTime,
      string EndDate,
      string EndTime,
      string? Owner_id,
      int? status_id,
      DateTime ExpirationDate,
      int? TaskLevel_id,
      DateTime lastModifiedDate
  );
}
