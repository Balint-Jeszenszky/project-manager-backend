using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project_manager_backend.Models
{
    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } // hash
        public string Email { get; set; }

        public ICollection<Project> Projects { get; set; }
    }
}
