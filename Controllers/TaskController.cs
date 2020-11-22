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
    [Route("api/Tasks")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ProjectManagerDBContext context;
        public TaskController(ProjectManagerDBContext context)
        {
            this.context = context;
        }

        [HttpGet("task/{taskId}")]
        public async Task<ActionResult<Models.Task>> GetTask(int taskID)
        {
            var task = await context.Tasks.FindAsync(taskID);

            if (task == null)
            {
                return NotFound();
            }

            return task;
        }

        [HttpGet("group/{groupID}")]
        public async Task<ActionResult<IEnumerable<Models.Task>>> GetGroup(int groupID)
        {
            return await context.Tasks.Where(t => t.TaskgroupID == groupID).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Models.Task>> PostProject(Models.Task task)
        {
            context.Tasks.Add(task);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTask), new { taskId = task.ID }, task);
        }

        [HttpPut("{taskId}")]
        public async Task<IActionResult> UpdateProject(int taskId, Models.Task task)
        {
            if (taskId != task.ID)
            {
                return BadRequest();
            }

            var oldtask = await context.Tasks.FindAsync(taskId);

            if (oldtask.Priority != task.Priority)
            {
                var changetask = await context.Tasks.FirstAsync(t => t.Priority == task.Priority && t.TaskgroupID == task.TaskgroupID);
                changetask.Priority = oldtask.Priority;
                oldtask.Priority = task.Priority;
                context.Entry(changetask).State = EntityState.Modified;
                context.Entry(oldtask).State = EntityState.Modified;
            }
            else
            {
                oldtask.Name = task.Name;
                oldtask.Description = task.Description;
                oldtask.Deadline = task.Deadline;
                if (oldtask.TaskgroupID != task.TaskgroupID)
                {
                    (await context.Tasks.Where(t => t.Priority > oldtask.Priority && t.TaskgroupID == oldtask.TaskgroupID).ToListAsync()).ForEach(g => g.Priority -= 1);
                    oldtask.Priority = ((await context.Tasks.Where(t => t.TaskgroupID == task.TaskgroupID).MaxAsync(t => (int?)t.Priority)) ?? 0) + 1;
                    oldtask.TaskgroupID = task.TaskgroupID;
                }
                context.Entry(oldtask).State = EntityState.Modified;
            }

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(taskId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        [HttpDelete("{taskId}")]
        public async Task<ActionResult<Models.Task>> DeleteProject(int taskId)
        {
            var delTask = await context.Tasks.FindAsync(taskId);
            if (delTask == null)
            {
                return NotFound();
            }

            (await context.Tasks.Where(t => t.Priority > delTask.Priority && t.TaskgroupID == delTask.TaskgroupID).ToListAsync()).ForEach(g => g.Priority -= 1);

            context.Tasks.Remove(delTask);
            await context.SaveChangesAsync();

            return delTask;
        }
        private bool TaskExists(int id) =>
            context.Tasks.Any(e => e.ID == id);
    }
}
