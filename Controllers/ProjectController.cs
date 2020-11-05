using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project_manager_backend.Models;

namespace project_manager_backend.Controllers
{
    [Route("api/Project")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private ProjectManagerDBContext context;
        public ProjectController(ProjectManagerDBContext context)
        {
           this.context = context;
        }

        [HttpGet("{projectId}")]
        public async Task<ActionResult<Models.Project>> GetTodoItem(int projectId)
        {
            var project = await context.Projects.FindAsync(projectId);

            if (project == null)
            {
                return NotFound();
            }

            return project;
        }
    }
}
