// <copyright file="TaskGroup.cs" company="BME VIK AUT Témalaboratóruim">
// Copyright (c) BME VIK AUT Témalaboratóruim. All rights reserved.
// </copyright>

namespace Project_manager_backend.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// TaskGroup.
    /// </summary>
    public class TaskGroup
    {
        /// <summary>
        /// Gets or sets ID of TaskGroup.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets ProjectID of TaskGroup.
        /// </summary>
        public int ProjectID { get; set; }

        /// <summary>
        /// Gets or sets Name of TaskGroup.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Priority of TaskGroup.
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets Project containing this TaskGroup.
        /// </summary>
        public Project Project { get; set; }

        /// <summary>
        /// Gets or sets Tasks in this TaskGroup.
        /// </summary>
        public ICollection<Task> Tasks { get; set; }
    }
}
