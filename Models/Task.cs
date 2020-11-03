using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project_manager_backend.Models
{
    public class Task
    {
        public int ID { get; set; }
        public int TaskgroupID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public DateTime Deadline { get; set; }

        public TaskGroup TaskGroup { get; set; }
    }
}
