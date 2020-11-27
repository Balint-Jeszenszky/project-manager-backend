// <copyright file="UserController.cs" company="BME VIK AUT Témalaboratóruim">
// Copyright (c) BME VIK AUT Témalaboratóruim. All rights reserved.
// </copyright>

namespace Project_manager_backend.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Project_manager_backend.Models;

    /// <summary>
    /// api/User endpoint, responsible for User CRUD.
    /// </summary>
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ProjectManagerDBContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="context">ProjectManagerDBContext instance.</param>
        public UserController(ProjectManagerDBContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Handle GET request for the user.
        /// </summary>
        /// <param name="userID">ID of the user.</param>
        /// <returns>Returns the requested user.</returns>
        [HttpGet("{userID}")]
        public async Task<ActionResult<User>> GetUser(int userID)
        {
            var user = await this.context.Users.FindAsync(userID);

            if (user == null)
            {
                return this.NotFound();
            }

            return user;
        }

        /// <summary>
        /// Handle POST request for a new user reg.
        /// </summary>
        /// <param name="user">The new user.</param>
        /// <returns>Returns the new user or conflict if the username is alredy exists.</returns>
        [HttpPost("register")]
        public async Task<ActionResult<User>> AddUser(User user)
        {
            var founduser = await this.context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == user.Username);
            if (founduser != null)
            {
                return this.Conflict();
            }

            this.context.Users.Add(user);
            await this.context.SaveChangesAsync();

            return this.CreatedAtAction(nameof(this.GetUser), new { userID = user.ID }, user);
        }

        /// <summary>
        /// Handle POST request for login.
        /// </summary>
        /// <param name="user">User credentials.</param>
        /// <returns>Unauthorized if the credentials wrong or the users data.</returns>
        [HttpPost("login")]
        public async Task<ActionResult<User>> CheckUser(User user)
        {
            var founduser = await this.context.Users.FirstOrDefaultAsync(u => u.Username == user.Username && u.Password == user.Password);

            if (founduser == null)
            {
                return this.Unauthorized();
            }

            return founduser;
        }

        /// <summary>
        /// Handle PUT request for updating user data.
        /// </summary>
        /// <param name="userID">ID of user.</param>
        /// <param name="user">Modified user data.</param>
        /// <returns>Returns BadRequest if the ID is wrong or Nocontext.</returns>
        [HttpPut("{userID}")]
        public async Task<IActionResult> UpdateUser(int userID, User user)
        {
            if (userID != user.ID)
            {
                return this.BadRequest();
            }

            var savedUser = await this.context.Users.FindAsync(userID);

            savedUser.Name = user.Name;
            savedUser.Email = user.Email;
            if (user.Password != string.Empty)
            {
                savedUser.Password = user.Password;
            }

            try
            {
                await this.context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.UserExists(userID))
                {
                    return this.NotFound();
                }
                else
                {
                    throw;
                }
            }

            return this.NoContent();
        }

        /// <summary>
        /// Handle DELETE request for deleting a user.
        /// </summary>
        /// <param name="userID">ID of the user.</param>
        /// <returns>Returns the deleted user.</returns>
        [HttpDelete("{userID}")]
        public async Task<ActionResult<User>> DeleteUser(int userID)
        {
            var delUser = await this.context.Users.FindAsync(userID);
            if (delUser == null)
            {
                return this.NotFound();
            }

            this.context.Users.Remove(delUser);
            await this.context.SaveChangesAsync();

            return delUser;
        }

        private bool UserExists(int id) =>
            this.context.Users.Any(e => e.ID == id);
    }
}
