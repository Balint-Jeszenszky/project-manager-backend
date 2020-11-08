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
        public async Task<ActionResult<IEnumerable<Models.Task>>> GetProject(int taskId)
        {
            return await context.Tasks.Where(t => t.ID == taskId).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Models.Task>> PostProject(Models.Task task)
        {
            context.Tasks.Add(task);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProject), new { id = task.ID }, task);
        }

        [HttpPut("{taskId}")]
        public async Task<IActionResult> UpdateProject(int taskId, Models.Task task)
        {
            if (taskId != task.ID)
            {
                return BadRequest();
            }

            context.Entry(task).State = EntityState.Modified;

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

            context.Tasks.Remove(delTask);
            await context.SaveChangesAsync();

            return delTask;
        }
        private bool TaskExists(int id) =>
            context.Tasks.Any(e => e.ID == id);
    }
}
