using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeApp.Models
{
    public class RoleTypeContext: DbContext
    {
        public RoleTypeContext(DbContextOptions<RoleTypeContext> options) : base(options)
        {

        }

        public DbSet<RoleType> RoleType { get; set; }

    }
}
