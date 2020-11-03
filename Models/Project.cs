using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project_manager_backend.Models
{
    public class Project
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public User User { get; set; }

        public ICollection<TaskGroup> TaskGroups { get; set; }
    }
}
