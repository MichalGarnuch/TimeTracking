﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TimeTrackingAPI.Models
{
    public partial class TagEntity
    {
        public TagEntity()
        {
            Tasks = new HashSet<TaskEntity>();
        }
        [Key]
        public int TagId { get; set; }
        public string TagName { get; set; }

        public virtual ICollection<TaskEntity> Tasks { get; set; }
        public ICollection<TaskTagEntity> TaskTags { get; set; }

    }
}