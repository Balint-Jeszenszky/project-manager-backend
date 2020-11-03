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
    [Route("api/[controller]/{userID}")]
    [ApiController]
    public class Projects : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Project>>> Get()
        {
            using (var context = new ProjectManagerDBContext())
            {
                return await context.Projects.ToListAsync();
            }
        }
    }
}
