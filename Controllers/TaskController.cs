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
    [Route("api/Task")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private ProjectManagerDBContext context;
        public TaskController(ProjectManagerDBContext context)
        {
            this.context = context;
        }

        [HttpGet("{taskId}")]
        public async Task<ActionResult<Models.Task>> GetTodoItem(int taskId)
        {
            var task = await context.Tasks.FirstOrDefaultAsync(t => t.ID == taskId);

            if (task == null)
            {
                return NotFound();
            }

            return task;
        }
    }
}
