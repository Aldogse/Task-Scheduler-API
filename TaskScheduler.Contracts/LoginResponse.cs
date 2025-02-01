using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskScheduler.Contracts
{
    public record LoginResponse(
      string FirstName,
      string LastName,
      string Email,
      string Token
    );
    
    
}
