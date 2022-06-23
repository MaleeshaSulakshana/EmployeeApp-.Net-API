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
    public class RoleTypesController : ControllerBase
    {
        private readonly RoleTypeContext _context;

        public RoleTypesController(RoleTypeContext context)
        {
            _context = context;
        }

        // Route for get all role type details
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleType>>> GetRoleType()
        {
            return await _context.RoleType.ToListAsync();
        }

        // Route get role type details by id
        [HttpGet("{id}")]
        public async Task<ActionResult<RoleType>> GetRoleType(int id)
        {
            var roleType = await _context.RoleType.FindAsync(id);

            if (roleType == null)
            {
                return NotFound();
            }

            return roleType;
        }

        // Route for delete role tyoe details
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoleType(int id, RoleType roleType)
        {
            if (id != roleType.RoleID)
            {
                return BadRequest();
            }

            _context.Entry(roleType).State = EntityState.Modified;
            _context.Entry(roleType).Property(roleType => roleType.CreatedAt).IsModified = false;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleTypeExists(id))
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

        // Route for add new role type recode
        [HttpPost]
        public async Task<ActionResult<RoleType>> PostRoleType(RoleType roleType)
        {
            _context.RoleType.Add(roleType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRoleType", new { id = roleType.RoleID }, roleType);
        }

        // Route for delete role type
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoleType(int id)
        {
            var roleType = await _context.RoleType.FindAsync(id);
            if (roleType == null)
            {
                return NotFound();
            }

            _context.RoleType.Remove(roleType);
            await _context.SaveChangesAsync();

            return Ok(new { status = "Delete successfull" });
        }

        private bool RoleTypeExists(int id)
        {
            return _context.RoleType.Any(e => e.RoleID == id);
        }
    }
}
