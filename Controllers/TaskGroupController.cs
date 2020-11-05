using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project_manager_backend.Models;

namespace project_manager_backend.Controllers
{
    [Route("api/TaskGroup")]
    [ApiController]
    public class TaskGroupController : ControllerBase
    {
        private ProjectManagerDBContext context;
        public TaskGroupController(ProjectManagerDBContext context)
        {
            this.context = context;
        }

        [HttpGet("{groupId}")]
        public async Task<ActionResult<Models.TaskGroup>> GetTodoItem(int groupId)
        {
            var taskgroup = await context.TaskGroups.FindAsync(groupId);

            if (taskgroup == null)
            {
                return NotFound();
            }

            return taskgroup;
        }
    }
}
