// <copyright file="TaskGroupController.cs" company="BME VIK AUT Témalaboratóruim">
// Copyright (c) BME VIK AUT Témalaboratóruim. All rights reserved.
// </copyright>

namespace Project_manager_backend.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Project_manager_backend.Models;

    /// <summary>
    /// api/TaskGroup endpoint, responsible for TaskGroup CRUD.
    /// </summary>
    [Route("api/TaskGroup")]
    [ApiController]
    public class TaskGroupController : ControllerBase
    {
        private readonly ProjectManagerDBContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskGroupController"/> class.
        /// </summary>
        /// <param name="context">ProjectManagerDBContext instance.</param>
        public TaskGroupController(ProjectManagerDBContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Handle GET request for a specific taskgroup.
        /// </summary>
        /// <param name="groupID">ID fo the group.</param>
        /// <returns>Returns the group or NotFound.</returns>
        [HttpGet("group/{groupID}")]
        public async Task<ActionResult<TaskGroup>> GetGroup(int groupID)
        {
            var group = await this.context.TaskGroups.FindAsync(groupID);

            if (group == null)
            {
                return this.NotFound();
            }

            return group;
        }

        /// <summary>
        /// Handle GET request for all taskgroups in a project.
        /// </summary>
        /// <param name="projectId">ID of the project.</param>
        /// <returns>Returns a list of taskgroups.</returns>
        [HttpGet("groups/{projectId}")]
        public async Task<ActionResult<IEnumerable<TaskGroup>>> GetTaskGroups(int projectId)
        {
            return await this.context.TaskGroups.Where(t => t.ProjectID == projectId).ToListAsync();
        }

        /// <summary>
        /// Handle POST request for creating a new taskgroup.
        /// </summary>
        /// <param name="taskGroup">The new taskgroup.</param>
        /// <returns>Returns the created taskgroup.</returns>
        [HttpPost]
        public async Task<ActionResult<TaskGroup>> PostProject(TaskGroup taskGroup)
        {
            this.context.TaskGroups.Add(taskGroup);
            await this.context.SaveChangesAsync();

            return this.CreatedAtAction(nameof(this.GetGroup), new { groupID = taskGroup.ID }, taskGroup);
        }

        /// <summary>
        /// Handle PUT request for updating a taskgroup.
        /// </summary>
        /// <param name="groupID">ID of the updated taskgroup.</param>
        /// <param name="taskGroup">The updated taskgroup.</param>
        /// <returns>Returns a BadRequest if the groupID not equal with the taskgroup ID or NoContext.</returns>
        [HttpPut("{groupID}")]
        public async Task<IActionResult> UpdateProject(int groupID, TaskGroup taskGroup)
        {
            if (groupID != taskGroup.ID)
            {
                return this.BadRequest();
            }

            var oldgroup = await this.context.TaskGroups.FindAsync(groupID);

            if (oldgroup.Priority != taskGroup.Priority)
            {
                var changegroup = await this.context.TaskGroups.FirstAsync(g => g.Priority == taskGroup.Priority && g.ProjectID == taskGroup.ProjectID);
                changegroup.Priority = oldgroup.Priority;
                oldgroup.Priority = taskGroup.Priority;
                this.context.Entry(changegroup).State = EntityState.Modified;
                this.context.Entry(oldgroup).State = EntityState.Modified;
            }
            else
            {
                oldgroup.Name = taskGroup.Name;
                this.context.Entry(oldgroup).State = EntityState.Modified;
            }

            try
            {
                await this.context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.GroupExists(groupID))
                {
                    return this.NotFound();
                }
                else
                {
                    throw;
                }
            }

            return this.NoContent();
        }

        /// <summary>
        /// Handle DELETE request for deleting a taskgroup.
        /// </summary>
        /// <param name="groupID">ID of group to delete.</param>
        /// <returns>Returns the deleted group.</returns>
        [HttpDelete("{groupID}")]
        public async Task<ActionResult<TaskGroup>> DeleteProject(int groupID)
        {
            var delGroup = await this.context.TaskGroups.FindAsync(groupID);
            if (delGroup == null)
            {
                return this.NotFound();
            }

            (await this.context.TaskGroups.Where(g => g.Priority > delGroup.Priority && g.ProjectID == delGroup.ProjectID).ToListAsync()).ForEach(g => g.Priority -= 1);

            this.context.TaskGroups.Remove(delGroup);
            await this.context.SaveChangesAsync();

            return delGroup;
        }

        private bool GroupExists(int id) =>
            this.context.Projects.Any(e => e.ID == id);
    }
}
