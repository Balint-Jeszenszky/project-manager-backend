// <copyright file="ProjectManagerDBContext.cs" company="BME VIK AUT Témalaboratóruim">
// Copyright (c) BME VIK AUT Témalaboratóruim. All rights reserved.
// </copyright>

namespace Project_manager_backend.Models
{
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Code first DB context with EF core.
    /// </summary>
    public class ProjectManagerDBContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectManagerDBContext"/> class.
        /// </summary>
        /// <param name="context">DbContextOptions.</param>
        public ProjectManagerDBContext(DbContextOptions context)
            : base(context)
        {
        }

        /// <summary>
        /// Gets or sets DBset of Users.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Gets or sets DBset of Projects.
        /// </summary>
        public DbSet<Project> Projects { get; set; }

        /// <summary>
        /// Gets or sets DBset of TaskGroups.
        /// </summary>
        public DbSet<TaskGroup> TaskGroups { get; set; }

        /// <summary>
        /// Gets or sets DBset of Tasks.
        /// </summary>
        public DbSet<Task> Tasks { get; set; }
    }
}
