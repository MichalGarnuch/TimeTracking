﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTrackingMobile.Models
{
    public class TaskTagModel
    {
        public int TaskID { get; set; }
        public string TaskName { get; set; }
        public int TagID { get; set; }
        public string TagName { get; set; }
    }
}
