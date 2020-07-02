using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System.Threading.Tasks;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _applicationContext;
        private IAccountRepository _accountRepository;
        private ILessonRepository _lessonRepository;
        private IThemeRepository _themeRepository;
        private ICourseRepository _courseRepository;
        private IMentorRepository _mentorRepository;
        private IMentorOfCourseRepository _mentorOfCourseRepository;

        public UnitOfWork(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public IAccountRepository AccountRepository
        {
            get { return _accountRepository = _accountRepository ?? new AccountRepository(_applicationContext); }
        }

        public ILessonRepository LessonRepository
        {
            get { return _lessonRepository = _lessonRepository ?? new LessonRepository(_applicationContext); }
        }

        public IThemeRepository ThemeRepository
        {
            get { return _themeRepository = _themeRepository ?? new ThemeRepository(_applicationContext); }
        }

        public ICourseRepository CourseRepository
        {
            get { return _courseRepository = _courseRepository ?? new CourseRepository(_applicationContext);  }
        }

        public IMentorRepository MentorRepository
        {
            get { return _mentorRepository = _mentorRepository ?? new MentorRepository(_applicationContext); }
        }

        public IMentorOfCourseRepository MentorOfCourseRepository
        {
            get { return _mentorOfCourseRepository = _mentorOfCourseRepository ?? new MentorOfCourseRepository(_applicationContext); }
        }


        public Task CommitAsync()
        {
            return _applicationContext.SaveChangesAsync();
        }

        public void Rollback()
        {
            _applicationContext.Dispose();
        }
    }
}
