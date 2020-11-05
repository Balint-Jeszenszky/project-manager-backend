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
    [Route("api/Projects")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private ProjectManagerDBContext context;
        public ProjectsController(ProjectManagerDBContext context)
        {
            this.context = context;
        }

        [HttpGet("{userID}")]
        public async Task<ActionResult<IEnumerable<Models.Project>>> Get()
        {
            return await context.Projects.ToListAsync();
        }
    }
}
