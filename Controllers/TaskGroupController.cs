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

        [HttpGet("group/{groupID}")]
        public async Task<ActionResult<TaskGroup>> GetGroup(int groupID)
        {
            var group = await context.TaskGroups.FindAsync(groupID);

            if (group == null)
            {
                return NotFound();
            }

            return group;
        }

        [HttpGet("groups/{projectId}")]
        public async Task<ActionResult<IEnumerable<TaskGroup>>> GetTaskGroups(int projectId)
        {
            return await context.TaskGroups.Where(t => t.ProjectID == projectId).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<TaskGroup>> PostProject(TaskGroup taskGroup)
        {
            context.TaskGroups.Add(taskGroup);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetGroup), new { groupID = taskGroup.ID }, taskGroup);
        }

        [HttpPut("{groupID}")]
        public async Task<IActionResult> UpdateProject(int groupID, TaskGroup taskGroup)
        {
            if (groupID != taskGroup.ID)
            {
                return BadRequest();
            }

            var oldgroup = await context.TaskGroups.FindAsync(groupID);

            if (oldgroup.Priority != taskGroup.Priority)
            {
                var changegroup = await context.TaskGroups.FirstAsync(g => g.Priority == taskGroup.Priority && g.ProjectID == taskGroup.ProjectID);
                changegroup.Priority = oldgroup.Priority;
                oldgroup.Priority = taskGroup.Priority;
                context.Entry(changegroup).State = EntityState.Modified;
                context.Entry(oldgroup).State = EntityState.Modified;
            }
            else
            {
                oldgroup.Name = taskGroup.Name;
                context.Entry(oldgroup).State = EntityState.Modified;
            }

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

            (await context.TaskGroups.Where(g => g.Priority > delGroup.Priority && g.ProjectID == delGroup.ProjectID).ToListAsync()).ForEach(g => g.Priority -= 1);

            context.TaskGroups.Remove(delGroup);
            await context.SaveChangesAsync();

            return delGroup;
        }
        private bool GroupExists(int id) =>
            context.Projects.Any(e => e.ID == id);
    }
}
