﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TimeTrackingAPI.Models
{
    public partial class TaskEntity
    {
        public TaskEntity()
        {
            TimeRecords = new HashSet<TimeRecordEntity>();
            Tags = new HashSet<TagEntity>();
        }
        [Key]
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
        public int ProjectId { get; set; }
        public string Status { get; set; }
        public int Priority { get; set; }

        public virtual ProjectEntity Project { get; set; }
        public virtual ICollection<TimeRecordEntity> TimeRecords { get; set; }

        public virtual ICollection<TagEntity> Tags { get; set; }
        public ICollection<TaskTagEntity> TaskTags { get; set; }

    }
}