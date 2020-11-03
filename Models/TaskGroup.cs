using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project_manager_backend.Models
{
    public class TaskGroup
    {
        public int ID { get; set; }
        public int ProjectID { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }

        public Project Project { get; set; }

        public ICollection<Task> Tasks { get; set; }
    }
}
