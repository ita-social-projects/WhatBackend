using CharlieBackend.Business.Services.FileServices.ExportFileServices;
using CharlieBackend.Core.DTO.Dashboard;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Core.DTO.Export
{
    public class StudentsRequestWithFileExtensionDto<T> where T : Enum
    {
        public long? CourseId { get; set; }

        public long? StudentGroupId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime FinishDate { get; set; }

        public T[] IncludeAnalytics { get; set; }

        public ExportFileExtension Extension { get; set; }

        public StudentsRequestDto<T> GetStudentsRequestDto()
        {
            return new StudentsRequestDto<T>
            {
                CourseId = CourseId,
                StudentGroupId = StudentGroupId,
                StartDate = StartDate,
                FinishDate = FinishDate,
                IncludeAnalytics = IncludeAnalytics
            };
        }
    }
}
