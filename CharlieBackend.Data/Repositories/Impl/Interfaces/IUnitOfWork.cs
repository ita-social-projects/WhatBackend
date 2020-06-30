namespace CharlieBackend.Data.Repositories.Impl.Interfaces
{
    public interface IUnitOfWork
    {
        public IAccountRepository AccountRepository { get; }
        public ILessonRepository LessonRepository { get; }
        public IThemeRepository ThemeRepository { get; }
        public ICourseRepository CourseRepository { get;  }

        void Commit();
        void Rollback();
    }
}
