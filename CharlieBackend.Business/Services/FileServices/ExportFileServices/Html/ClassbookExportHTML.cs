using CharlieBackend.Core.DTO.Dashboard;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices.ExportFileServices.Html
{
    class ClassbookExportHTML : BaseFileExportHTML
    {
        public ClassbookExportHTML()
        {

        }

        public async Task FillFile(StudentsClassbookResultDto data)
        {
            if (data == null)
            {
                return;
            }

            //if (data.StudentsMarks != null && data.StudentsMarks.Any())
            //{
            //    var StudentsMarks = data.StudentsMarks.GroupBy(x => x.StudentGroup);
            //    foreach (var item in StudentsMarks)
            //    {
            //        await TryToFillMarks(new StudentsClassbookResultDto
            //        {
            //            StudentsMarks = item
            //                 .Select(x => new StudentMarkDto
            //                 {
            //                     LessonDate = x.LessonDate,
            //                     LessonId = x.LessonId,
            //                     Course = x.Course,
            //                     StudentMark = x.StudentMark,
            //                     Student = x.Student,
            //                     StudentGroup = x.StudentGroup,
            //                     StudentId = x.StudentId,
            //                     Comment = x.Comment
            //                 })
            //                .ToList(),
            //            StudentsPresences = null
            //        });
            //    }
            //}

            //if (data.StudentsPresences != null && data.StudentsPresences.Any())
            //{
            //    var StudentsPresences = data.StudentsPresences.GroupBy(x => x.StudentGroup);
            //    foreach (var item in StudentsPresences)
            //    {
            //        await TryToFillPresences(new StudentsClassbookResultDto
            //        {
            //            StudentsMarks = null,
            //            StudentsPresences = item
            //                .Select(x => new StudentVisitDto
            //                {
            //                    LessonDate = x.LessonDate,
            //                    LessonId = x.LessonId,
            //                    Course = x.Course,
            //                    Presence = x.Presence,
            //                    Student = x.Student,
            //                    StudentGroup = x.StudentGroup,
            //                    StudentId = x.StudentId
            //                })
            //                .ToList()
            //        });
            //    }
            //}

            await Task.CompletedTask;
        }
        public override string GetFileName()
        {
            return "Classbook_" + DateTime.Now.ToString("yyyy-MM-dd.HH:mm") + ".html";
        }
    }
}
