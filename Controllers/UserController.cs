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

        [HttpPost]
        public async Task<ActionResult<User>> AddUser(User user)
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { userID = user.ID }, user);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser()
        {
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser()
        {
            return NoContent();
        }
    }
}
