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
    [Route("api/[controller]/{taskId}")]
    [ApiController]
    public class Task : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<Models.Task>> GetTodoItem(int taskId)
        {
            using (var context = new ProjectManagerDBContext())
            {
                var task = await context.Tasks.Include(t => t.TaskGroup).FirstOrDefaultAsync(t => t.ID == taskId);

                if (task == null)
                {
                    return NotFound();
                }

                return task;
            }
        }
    }
}
