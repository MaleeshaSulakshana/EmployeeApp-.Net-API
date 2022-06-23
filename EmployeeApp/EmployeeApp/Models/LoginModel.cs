using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeApp.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email required")]
        public string email { get; set; }

        [Required(ErrorMessage = "Password required")]
        public string password { get; set; }

    }
}
