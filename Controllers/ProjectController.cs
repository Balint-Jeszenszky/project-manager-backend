// <copyright file="ProjectController.cs" company="BME VIK AUT Témalaboratóruim">
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
    /// api/Project endpoint, responsible for Project CRUD.
    /// </summary>
    [Route("api/Project")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly ProjectManagerDBContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectController"/> class.
        /// </summary>
        /// <param name="context">ProjectManagerDBContext instance.</param>
        public ProjectController(ProjectManagerDBContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Handle GET request for the users all projects.
        /// </summary>
        /// <param name="userID">ID of the user.</param>
        /// <returns>Returns a list of projects.</returns>
        [HttpGet("projects/{userID}")]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects(int userID)
        {
            return await this.context.Projects.Where(p => p.UserID == userID).ToListAsync();
        }

        /// <summary>
        /// Handle GET request for a specific project.
        /// </summary>
        /// <param name="projectID">ID of the requested project.</param>
        /// <returns>Returns the requested project.</returns>
        [HttpGet("project/{projectID}")]
        public async Task<ActionResult<Project>> GetProject(int projectID)
        {
            var project = await this.context.Projects.FindAsync(projectID);

            if (project == null)
            {
                return this.NotFound();
            }

            return project;
        }

        /// <summary>
        /// Handle POST request for creating a project.
        /// </summary>
        /// <param name="project">The new project.</param>
        /// <returns>Returns the created project.</returns>
        [HttpPost]
        public async Task<ActionResult<Project>> PostProject(Project project)
        {
            this.context.Projects.Add(project);
            await this.context.SaveChangesAsync();

            return this.CreatedAtAction(nameof(this.GetProject), new { id = project.ID }, project);
        }

        /// <summary>
        /// Handle PUT request for updating a project.
        /// </summary>
        /// <param name="projectID">ID of the updated project.</param>
        /// <param name="project">The updated project.</param>
        /// <returns>Returns BadRequest if the ID is wrong or NoContext.</returns>
        [HttpPut("{projectID}")]
        public async Task<IActionResult> UpdateProject(int projectID, Project project)
        {
            if (projectID != project.ID)
            {
                return this.BadRequest();
            }

            this.context.Entry(project).State = EntityState.Modified;

            try
            {
                await this.context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.ProjectExists(projectID))
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
        /// Handle DELETE request for deleting a project.
        /// </summary>
        /// <param name="projectID">ID of the  project to delete.</param>
        /// <returns>Returns the deleted project.</returns>
        [HttpDelete("{projectID}")]
        public async Task<ActionResult<Project>> DeleteProject(int projectID)
        {
            var delProject = await this.context.Projects.FindAsync(projectID);
            if (delProject == null)
            {
                return this.NotFound();
            }

            this.context.Projects.Remove(delProject);
            await this.context.SaveChangesAsync();

            return delProject;
        }

        private bool ProjectExists(int id) =>
            this.context.Projects.Any(e => e.ID == id);
    }
}
