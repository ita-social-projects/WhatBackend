using CharlieBackend.Core.DTO.Dashboard;
using System.Linq;

namespace CharlieBackend.Business.Services.FileServices.ExportFileServices.Html
{
    class StudentClassbookExportHTML : BaseFileExportHTML
    {
        public void FillFile(StudentsClassbookResultDto data)
        {
            if (data == null)
            {
                return;
            }
            
            if(data.StudentsMarks != null && data.StudentsMarks.Any())
            {

            }
        }
    }
}
