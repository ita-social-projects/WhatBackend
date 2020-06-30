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


        public void Commit()
        {
            _applicationContext.SaveChanges();
        }

        public void Rollback()
        {
            _applicationContext.Dispose();
        }
    }
}
