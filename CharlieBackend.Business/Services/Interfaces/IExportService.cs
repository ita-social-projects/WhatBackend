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
        Task<ClassbookExport> GetStudentsClassbook(StudentsRequestDto<ClassbookResultType> request);

        Task<StudentsResultsExport> GetStudentsResults(StudentsRequestDto<StudentResultType> request);

        Task<StudentClassbook> GetStudentClassbook(long studentId, DashboardAnalyticsRequestDto<ClassbookResultType> request);

        Task<StudentResult> GetStudentResults(long studentId, DashboardAnalyticsRequestDto<StudentResultType> request);

        Task<StudentGroupResults> GetStudentGroupResults(long courseId, DashboardAnalyticsRequestDto<StudentGroupResultType> request);
    }
}
