﻿using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.DTO.Student
{
    public class StudentStudyGroupsDto
    {
        public long Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }
    }
}
