using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeApp.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace EmployeeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDetailsController : ControllerBase
    {
        private readonly UserDetailsContext _context;
        public UserEntitiesContext _userEntitiesContext;

        public UserDetailsController(UserDetailsContext context, UserEntitiesContext userEntitiesContext)
        {
            _context = context;
            _userEntitiesContext = userEntitiesContext;
        }

        // Route for get all user details
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDetails>>> GetUserDetails()
        {
            return await _context.UserDetails.ToListAsync();
        }

        // Route for get user details by id
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDetails>> GetUserDetails(int id)
        {
            var userDetails = await _context.UserDetails.FindAsync(id);

            if (userDetails == null)
            {
                return NotFound();
            }

            return userDetails;
        }

        // Route for get user details by email with role name and status name
        [Authorize]
        [HttpGet("/api/userInfo/{email}")]
        public async Task<ActionResult<UserDetailsDTO>> GetUserInfo(string email)
        {
            // Get user information from db with role name and status name
            var userDetails = _userEntitiesContext.UserDetails
                .Join(_userEntitiesContext.RoleType,
                    user => user.RoleType,
                    role => role.RoleID,
                    (user, role) => new {user, role})
                .Join(_userEntitiesContext.Status,
                    combinedObj => combinedObj.user.Status,
                    status => status.StatusID,
                    (combinedObj, status) => new UserDetailsDTO()
                    {
                        UserID = combinedObj.user.UserID,
                        FirstName = combinedObj.user.FirstName,
                        LastName = combinedObj.user.LastName,
                        Email = combinedObj.user.Email,
                        Password = combinedObj.user.Password,
                        DateofBirth = combinedObj.user.DateofBirth,
                        RoleType = combinedObj.role.RoleName,
                        Status = status.StatusName
                    }).Where(u => u.Email == email).ToList()[0];

            if (userDetails == null)
            {
                return NotFound();
            }

            return userDetails;
        }

        // Route for update user details
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserDetails(int id, UserDetails userDetails)
        {
            if (id != userDetails.UserID)
            {
                return BadRequest();
            }

            _context.Entry(userDetails).State = EntityState.Modified;

            _context.Entry(userDetails).Property(user => user.Password).IsModified = false;
            _context.Entry(userDetails).Property(user => user.CreatedAt).IsModified = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserDetailsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { status = "Updated successfull"});
        }

        // Route for add user details to db
        [HttpPost]
        public async Task<ActionResult<UserDetails>> PostUserDetails(UserDetails userDetails)
        {

            if (UserDetailsExistsByEmail(userDetails.Email))
            {
                return BadRequest("This email is already exist!");
            }

            // Validate email
            var isValid = new EmailAddressAttribute().IsValid(userDetails.Email);
            if (!isValid) { return BadRequest("Your email not valid. Please check your email"); }

            // Hash password
            userDetails.Password = BCrypt.Net.BCrypt.HashPassword(userDetails.Password);

            _context.UserDetails.Add(userDetails);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserDetails", new { id = userDetails.UserID }, userDetails);
        }

        // Route for delete
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserDetails(int id)
        {
            var userDetails = await _context.UserDetails.FindAsync(id);
            if (userDetails == null)
            {
                return NotFound();
            }

            _context.UserDetails.Remove(userDetails);
            await _context.SaveChangesAsync();

            return Ok(new { status = "Delete successfull" });
        }

        // Method for check user details is exist 
        private bool UserDetailsExists(int id)
        {
            return _context.UserDetails.Any(e => e.UserID == id);
        }

        // Method for check email is exist
        private bool UserDetailsExistsByEmail(string email)
        {
            return _context.UserDetails.Any(e => e.Email == email);
        }

    }
}
