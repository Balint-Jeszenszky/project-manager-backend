using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project_manager_backend.Models;

namespace project_manager_backend.Controllers
{
    [Route("api/[controller]/{groupId}")]
    [ApiController]
    public class TaskGroup : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<Models.TaskGroup>> GetTodoItem(int groupId)
        {
            using (var context = new ProjectManagerDBContext())
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
}
