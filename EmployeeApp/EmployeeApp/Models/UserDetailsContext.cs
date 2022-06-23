using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeApp.Models
{
    public class UserDetailsContext:DbContext
    {

        public UserDetailsContext(DbContextOptions<UserDetailsContext> options):base(options)
        {
            
        }

        public DbSet<UserDetails> UserDetails { get; set; }

    }
}
