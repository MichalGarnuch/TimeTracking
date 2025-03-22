using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTrackingMobile.Models
{
    public class Task
    {
        public int TaskID { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
        public int ProjectID { get; set; }
        public string Status { get; set; }
        public int Priority { get; set; }
    }
}
