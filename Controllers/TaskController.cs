// <copyright file="TaskController.cs" company="BME VIK AUT Témalaboratóruim">
// Copyright (c) BME VIK AUT Témalaboratóruim. All rights reserved.
// </copyright>

namespace Project_manager_backend.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Project_manager_backend.Models;

    /// <summary>
    /// api/Tasks endpoint, responsible for Task CRUD.
    /// </summary>
    [Route("api/Tasks")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ProjectManagerDBContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskController"/> class.
        /// </summary>
        /// <param name="context">ProjectManagerDBContext instance.</param>
        public TaskController(ProjectManagerDBContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Handle GET request for a specific task.
        /// </summary>
        /// <param name="taskID">The ID of the requested task.</param>
        /// <returns>Returns a Task or NotFound.</returns>
        [HttpGet("task/{taskId}")]
        public async Task<ActionResult<Models.Task>> GetTask(int taskID)
        {
            var task = await this.context.Tasks.FindAsync(taskID);

            if (task == null)
            {
                return this.NotFound();
            }

            return task;
        }

        /// <summary>
        /// Handle GET request for all tasks in a group.
        /// </summary>
        /// <param name="groupID">The ID of the requested tasks group.</param>
        /// <returns>Returns an array filled with tasks with the requested groupID.</returns>
        [HttpGet("group/{groupID}")]
        public async Task<ActionResult<IEnumerable<Models.Task>>> GetGroup(int groupID)
        {
            return await this.context.Tasks.Where(t => t.TaskgroupID == groupID).ToListAsync();
        }

        /// <summary>
        /// Handle POST request for adding a new task.
        /// </summary>
        /// <param name="task">Task to create.</param>
        /// <returns>Returns the created task.</returns>
        [HttpPost]
        public async Task<ActionResult<Models.Task>> PostProject(Models.Task task)
        {
            this.context.Tasks.Add(task);
            await this.context.SaveChangesAsync();

            return this.CreatedAtAction(nameof(this.GetTask), new { taskId = task.ID }, task);
        }

        /// <summary>
        /// Handle PUT request for updating a task.
        /// </summary>
        /// <param name="taskId">ID of the updated task.</param>
        /// <param name="task">Modified task.</param>
        /// <returns>Returns a BadRequest if the ID not equal with the task id or NoContext.</returns>
        [HttpPut("{taskId}")]
        public async Task<IActionResult> UpdateProject(int taskId, Models.Task task)
        {
            if (taskId != task.ID)
            {
                return this.BadRequest();
            }

            var oldtask = await this.context.Tasks.FindAsync(taskId);

            if (oldtask.Priority != task.Priority)
            {
                var changetask = await this.context.Tasks.FirstAsync(t => t.Priority == task.Priority && t.TaskgroupID == task.TaskgroupID);
                changetask.Priority = oldtask.Priority;
                oldtask.Priority = task.Priority;
                this.context.Entry(changetask).State = EntityState.Modified;
                this.context.Entry(oldtask).State = EntityState.Modified;
            }
            else
            {
                oldtask.Name = task.Name;
                oldtask.Description = task.Description;
                oldtask.Deadline = task.Deadline;
                if (oldtask.TaskgroupID != task.TaskgroupID)
                {
                    (await this.context.Tasks.Where(t => t.Priority > oldtask.Priority && t.TaskgroupID == oldtask.TaskgroupID).ToListAsync()).ForEach(g => g.Priority -= 1);
                    oldtask.Priority = ((await this.context.Tasks.Where(t => t.TaskgroupID == task.TaskgroupID).MaxAsync(t => (int?)t.Priority)) ?? 0) + 1;
                    oldtask.TaskgroupID = task.TaskgroupID;
                }

                this.context.Entry(oldtask).State = EntityState.Modified;
            }

            try
            {
                await this.context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.TaskExists(taskId))
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
        /// Handle DELETE request for updating a task.
        /// </summary>
        /// <param name="taskId">ID of the task.</param>
        /// <returns>Returns the deleted task.</returns>
        [HttpDelete("{taskId}")]
        public async Task<ActionResult<Models.Task>> DeleteProject(int taskId)
        {
            var delTask = await this.context.Tasks.FindAsync(taskId);
            if (delTask == null)
            {
                return this.NotFound();
            }

            (await this.context.Tasks.Where(t => t.Priority > delTask.Priority && t.TaskgroupID == delTask.TaskgroupID).ToListAsync()).ForEach(g => g.Priority -= 1);

            this.context.Tasks.Remove(delTask);
            await this.context.SaveChangesAsync();

            return delTask;
        }

        private bool TaskExists(int id) =>
            this.context.Tasks.Any(e => e.ID == id);
    }
}
