using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TaskScheduler.Contracts
{
    public  record CurrentUserDataResponse(
     string first_name,
     string last_name,
     string email,
     string role
    );
    
    
}
