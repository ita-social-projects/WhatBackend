using CharlieBackend.Business.Services.FileServices.ExportFileServices;
using CharlieBackend.Core.DTO.Dashboard;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IExportService
    {
        Task<ClassbookExportXlsx> GetStudentsClassbook(StudentsRequestDto<ClassbookResultType> request);

        Task<StudentsResultsExportXlsx> GetStudentsResults(StudentsRequestDto<StudentResultType> request);

        Task<StudentClassbookXlsx> GetStudentClassbook(long studentId, DashboardAnalyticsRequestDto<ClassbookResultType> request);

        Task<StudentResultXlsx> GetStudentResults(long studentId, DashboardAnalyticsRequestDto<StudentResultType> request);

        Task<StudentGroupResultsXlsx> GetStudentGroupResults(long courseId, DashboardAnalyticsRequestDto<StudentGroupResultType> request);
    }
}
