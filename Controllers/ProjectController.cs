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
        public async Task<ActionResult<IEnumerable<TaskGroup>>> GetTaskGroups(int projectId)
        {
            return await context.TaskGroups.Where(t => t.ProjectID == projectId).ToListAsync();
        }
    }
}
