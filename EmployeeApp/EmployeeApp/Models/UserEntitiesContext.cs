using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeApp.Models
{
    public class UserEntitiesContext:DbContext
    {

        public UserEntitiesContext(DbContextOptions<UserEntitiesContext> options) : base(options)
        {

        }

        public DbSet<UserDetails> UserDetails { get; set; }
        public DbSet<RoleType> RoleType { get; set; }
        public DbSet<Status> Status { get; set; }

    }
}
