using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskScheduler.Contracts
{
    public record UserInfoRequestResponse(
    
         string UserId ,
         string UserName ,
         string Email ,
         string FirstName ,
         string LastName,
         string user_role
    );
}
