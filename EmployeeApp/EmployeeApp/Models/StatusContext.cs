using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeApp.Models
{
    public class StatusContext: DbContext
    {
        public StatusContext(DbContextOptions<StatusContext> options) : base(options)
        {

        }

        public DbSet<Status> Status { get; set; }
    }
}
