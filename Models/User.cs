// <copyright file="User.cs" company="BME VIK AUT Témalaboratóruim">
// Copyright (c) BME VIK AUT Témalaboratóruim. All rights reserved.
// </copyright>

namespace Project_manager_backend.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// User.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets ID.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets name of user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the users projects.
        /// </summary>
        public ICollection<Project> Projects { get; set; }
    }
}
