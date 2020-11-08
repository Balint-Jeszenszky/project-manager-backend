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
    [Route("api/Projects")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private ProjectManagerDBContext context;
        public ProjectsController(ProjectManagerDBContext context)
        {
            this.context = context;
        }

        [HttpGet("{userID}")]
        public async Task<ActionResult<IEnumerable<Project>>> GetProject(int userID)
        {
            return await context.Projects.Where(p => p.UserID == userID).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Project>> PostProject(Project project)
        {
            context.Projects.Add(project);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProject), new { id = project.ID}, project);
        }

        [HttpPut("{projectID}")]
        public async Task<IActionResult> UpdateProject(int projectID, Project project)
        {
            if (projectID != project.ID)
            {
                return BadRequest();
            }

            context.Entry(project).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(projectID))
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

        [HttpDelete("{projectID}")]
        public async Task<ActionResult<Project>> DeleteProject(int projectID)
        {
            var delProject = await context.Projects.FindAsync(projectID);
            if (delProject == null)
            {
                return NotFound();
            }

            context.Projects.Remove(delProject);
            await context.SaveChangesAsync();

            return delProject;
        }
        private bool ProjectExists(int id) =>
            context.Projects.Any(e => e.ID == id);
    }
}
