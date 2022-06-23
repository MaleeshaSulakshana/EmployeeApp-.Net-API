using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeApp.Models
{
    public class UserDetails
    {

        [Key]
        public int UserID { get; set; }

        //[Column (TypeName = "string(255)")]
        public string FirstName { get; set; }

        //[Column(TypeName = "string(255)")]
        public string LastName { get; set; }
        
        //[Column (TypeName = "string(255)")]
        public string Email { get; set; }

        //[Column(TypeName = "string(255)")]
        public string Password { get; set; }

        //[Column(TypeName = "string(255)")]
        public string DateofBirth { get; set; }

        //[Column(TypeName = "int")]
        public int RoleType { get; set; }

        //[Column(TypeName = "int")]
        public int Status { get; set; }

        //[Column(TypeName = "DateTime")]
        public DateTime CreatedAt { get; set; }

        //[Column(TypeName = "DateTime")]
        public DateTime ModifiedAt { get; set; }

    }
}
