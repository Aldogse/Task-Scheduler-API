using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskScheduler.Contracts
{
    public record RegisterAccountRequest
    {
        [Required]
        public string first_name { get; set; }
        [Required]
        public string last_name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password not matched")]
        public string confirm_password { get; set; }
    }


}
