// <copyright file="Project.cs" company="BME VIK AUT Témalaboratóruim">
// Copyright (c) BME VIK AUT Témalaboratóruim. All rights reserved.
// </copyright>

namespace Project_manager_backend.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Project.
    /// </summary>
    public class Project
    {
        /// <summary>
        /// Gets or sets ID of Project.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets UserID of Project.
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// Gets or sets Name of Project.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Description of Project.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets User who owns the Project.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Gets or sets taskgroups in the Project.
        /// </summary>
        public ICollection<TaskGroup> TaskGroups { get; set; }
    }
}
