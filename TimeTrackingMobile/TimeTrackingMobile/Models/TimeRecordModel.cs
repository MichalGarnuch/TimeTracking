﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTrackingMobile.Models
{
    public class TimeRecordModel
    {
        public int TimeRecordID { get; set; }
        public int EmployeeID { get; set; }
        public int TaskID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal HoursSpent { get; set; }

        // nazwy dla wygodnego wyświetlania w aplikacji
        public string EmployeeName { get; set; }
        public string TaskName { get; set; }
    }
}
