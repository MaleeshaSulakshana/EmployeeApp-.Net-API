using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace EmployeeApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly StatusContext _context;

        public StatusController(StatusContext context)
        {
            _context = context;
        }

        // Route for get all status details
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Status>>> GetRoleType()
        {
            return await _context.Status.ToListAsync();
        }

        // Route for get status details by id
        [HttpGet("{id}")]
        public async Task<ActionResult<Status>> GetStatus(int id)
        {
            var status = await _context.Status.FindAsync(id);

            if (status == null)
            {
                return NotFound();
            }

            return status;
        }

        // Route for update status details
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStatus(int id, Status status)
        {
            if (id != status.StatusID)
            {
                return BadRequest();
            }

            _context.Entry(status).State = EntityState.Modified;
            _context.Entry(status).Property(status => status.CreatedAt).IsModified = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StatusExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { status = "Updated successfull" });
        }

        // Route for add new status record
        [HttpPost]
        public async Task<ActionResult<Status>> PostStatus(Status status)
        {
            _context.Status.Add(status);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStatus", new { id = status.StatusID }, status);
        }

        // Route for delete status recode
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStatus(int id)
        {
            var status = await _context.Status.FindAsync(id);
            if (status == null)
            {
                return NotFound();
            }

            _context.Status.Remove(status);
            await _context.SaveChangesAsync();

            return Ok(new { status = "Delete successfull" });
        }

        private bool StatusExists(int id)
        {
            return _context.Status.Any(e => e.StatusID == id);
        }
    }
}
