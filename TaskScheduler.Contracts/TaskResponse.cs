using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TaskScheduler.Contracts
{
    public  record TaskResponse(
        int id,
        string taskName,
        string Description,
        string StartDateAndTime,
        string EndDateAndTime,
        string? Owner_name,
        string? current_status,      
        string ExpirationDate,
        string? task_level
    );
    
    
}
