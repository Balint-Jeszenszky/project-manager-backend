// <copyright file="Task.cs" company="BME VIK AUT Témalaboratóruim">
// Copyright (c) BME VIK AUT Témalaboratóruim. All rights reserved.
// </copyright>

namespace Project_manager_backend.Models
{
    using System;

    /// <summary>
    /// Task.
    /// </summary>
    public class Task
    {
        /// <summary>
        /// Gets or sets ID of Task.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets TaskgroupID of Task.
        /// </summary>
        public int TaskgroupID { get; set; }

        /// <summary>
        /// Gets or sets Name of Task.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Description of Task.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets Priority of Task.
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets Deadline of Task.
        /// </summary>
        public DateTime Deadline { get; set; }

        /// <summary>
        /// Gets or sets TaskGroup containing this Task.
        /// </summary>
        public TaskGroup TaskGroup { get; set; }
    }
}
