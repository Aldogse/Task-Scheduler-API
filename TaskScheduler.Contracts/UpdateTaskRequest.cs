using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TaskScheduler.Contracts
{
    public  record UpdateTaskRequest(
      string TaskName,
      string Description,
      DateTime StartDateAndTime,
      DateTime EndDateAndTime,
      string? Owner,
      int? status_id,
      DateTime ExpirationDate,
      int TaskLevel_id,
      DateTime lastModifiedDate
    );
    
    
}
