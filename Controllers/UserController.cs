using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project_manager_backend.Models;

namespace project_manager_backend.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private ProjectManagerDBContext context;
        public UserController(ProjectManagerDBContext context)
        {
            this.context = context;
        }

        [HttpGet("{userID}")]
        public async Task<ActionResult<User>> GetUser(int userID)
        {
            var user = await context.Users.FindAsync(userID);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> AddUser(User user)
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { userID = user.ID }, user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> CheckUser(User user)
        {
            var founduser = await context.Users.FirstOrDefaultAsync(u => u.Username == user.Username && u.Password == user.Password);

            if (founduser == null)
            {
                return Unauthorized();
            }

            return founduser;
        }

        [HttpPut("{userID}")]
        public async Task<IActionResult> UpdateUser(int userID, User user)
        {
            if (userID != user.ID)
            {
                return BadRequest();
            }

            context.Entry(user).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(userID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        [HttpDelete("{userID}")]
        public async Task<ActionResult<User>> DeleteUser(int userID)
        {
            var delUser = await context.Users.FindAsync(userID);
            if (delUser == null)
            {
                return NotFound();
            }

            context.Users.Remove(delUser);
            await context.SaveChangesAsync();

            return delUser;
        }

        private bool UserExists(int id) =>
            context.Users.Any(e => e.ID == id);
    }
}
