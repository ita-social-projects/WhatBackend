using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface IUnitOfWork
    {
        public IAccountRepository AccountRepository { get; }

        public IAttachmentRepository AttachmentRepository { get; }

        public ISecretaryRepository SecretaryRepository { get; }

        public IScheduleRepository ScheduleRepository { get; }

        public ILessonRepository LessonRepository { get; }

        public IThemeRepository ThemeRepository { get; }

        public ICourseRepository CourseRepository { get; }

        public IMentorRepository MentorRepository { get; }

        public IMentorOfCourseRepository MentorOfCourseRepository { get; }

        public IStudentRepository StudentRepository { get; }

        public IStudentGroupRepository StudentGroupRepository { get; }

        public IVisitRepository VisitRepository { get; }

        public IDashboardRepository DashboardRepository { get; }

        public IHomeworkRepository HomeworkRepository { get; }

        Task CommitAsync();

        void Rollback();

        IDbContextTransaction BeginTransaction();
    }
}
