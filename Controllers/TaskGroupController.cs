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
    [Route("api/TaskGroup")]
    [ApiController]
    public class TaskGroupController : ControllerBase
    {
        private ProjectManagerDBContext context;
        public TaskGroupController(ProjectManagerDBContext context)
        {
            this.context = context;
        }

        [HttpGet("{groupID}")]
        public async Task<ActionResult<IEnumerable<Models.Task>>> GetGroup(int groupID)
        {
            return await context.Tasks.Where(t => t.TaskgroupID == groupID).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<TaskGroup>> PostProject(TaskGroup taskGroup)
        {
            context.TaskGroups.Add(taskGroup);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetGroup), new { id = taskGroup.ID }, taskGroup);
        }

        [HttpPut("{groupID}")]
        public async Task<IActionResult> UpdateProject(int groupID, TaskGroup taskGroup)
        {
            if (groupID != taskGroup.ID)
            {
                return BadRequest();
            }

            context.Entry(taskGroup).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupExists(groupID))
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

        [HttpDelete("{groupID}")]
        public async Task<ActionResult<TaskGroup>> DeleteProject(int groupID)
        {
            var delGroup = await context.TaskGroups.FindAsync(groupID);
            if (delGroup == null)
            {
                return NotFound();
            }

            context.TaskGroups.Remove(delGroup);
            await context.SaveChangesAsync();

            return delGroup;
        }
        private bool GroupExists(int id) =>
            context.Projects.Any(e => e.ID == id);
    }
}
