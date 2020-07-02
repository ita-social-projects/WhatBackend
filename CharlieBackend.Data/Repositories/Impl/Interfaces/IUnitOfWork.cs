using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface IUnitOfWork
    {
        public IAccountRepository AccountRepository { get; }
        public ILessonRepository LessonRepository { get; }
        public IThemeRepository ThemeRepository { get; }
        public ICourseRepository CourseRepository { get; }
        public IMentorRepository MentorRepository { get; }
        public IMentorOfCourseRepository MentorOfCourseRepository { get;  }

        Task CommitAsync();
        void Rollback();
    }
}
