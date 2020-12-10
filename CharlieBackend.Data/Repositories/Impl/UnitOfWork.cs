using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using CharlieBackend.Data.Repositories.Impl.Interfaces;


namespace CharlieBackend.Data.Repositories.Impl
{
    public class UnitOfWork : IUnitOfWork
    {
        #region private

        private readonly ApplicationContext _applicationContext;
        private IAccountRepository _accountRepository;
        private ISecretaryRepository _secretaryRepository;
        private ILessonRepository _lessonRepository;
        private IThemeRepository _themeRepository;
        private ICourseRepository _courseRepository;
        private IMentorRepository _mentorRepository;
        private IMentorOfCourseRepository _mentorOfCourseRepository;
        private IStudentRepository _studentRepository;
        private IStudentGroupRepository _studentGroupRepository;
        private IVisitRepository _visitRepository;
        private IDashboardRepository _dashboardRepository;
        private IScheduleRepository _scheduleRepository;
        private IAttachmentRepository _attachmentRepository;
        private IHometaskRepository _hometaskRepository;

        #endregion 

        public UnitOfWork(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public IAccountRepository AccountRepository
        {
            get
            {
                return _accountRepository = _accountRepository
                        ?? new AccountRepository(_applicationContext);
            }
        }

        public IAttachmentRepository AttachmentRepository
        {
             get
            {
                return _attachmentRepository = _attachmentRepository
                        ?? new AttachmentRepository(_applicationContext);
            }
        }
        public ISecretaryRepository SecretaryRepository
        {
            get
            {
                return _secretaryRepository = _secretaryRepository
                        ?? new SecretaryRepository(_applicationContext);
            }
        }

        public ILessonRepository LessonRepository
        {
            get 
            { 
                return _lessonRepository = _lessonRepository 
                        ?? new LessonRepository(_applicationContext);
            }
        }

        public IThemeRepository ThemeRepository
        {
            get
            {
                return _themeRepository = _themeRepository 
                        ?? new ThemeRepository(_applicationContext);
            }
        }

        public ICourseRepository CourseRepository
        {
            get 
            {
                return _courseRepository = _courseRepository 
                        ?? new CourseRepository(_applicationContext);
            }
        }

        public IMentorRepository MentorRepository
        {
            get
            { 
                return _mentorRepository = _mentorRepository 
                        ?? new MentorRepository(_applicationContext);
            }
        }

        public IMentorOfCourseRepository MentorOfCourseRepository
        {
            get
            { 
                return _mentorOfCourseRepository = _mentorOfCourseRepository 
                        ?? new MentorOfCourseRepository(_applicationContext);
            }
        }

        public IStudentRepository StudentRepository
        {
            get 
            { 
                return _studentRepository = _studentRepository 
                        ?? new StudentRepository(_applicationContext); 
            }
        }

        public IVisitRepository VisitRepository
        {
            get 
            { 
                return _visitRepository = _visitRepository 
                        ?? new VisitRepository(_applicationContext); 
            }
        }

        public IDashboardRepository DashboardRepository
        {
            get
            {
                return _dashboardRepository = _dashboardRepository
                        ?? new DashboardRepository(_applicationContext);
            }
        }

        public IScheduleRepository ScheduleRepository
        {
            get
            {
                return _scheduleRepository = _scheduleRepository
                        ?? new ScheduleRepository(_applicationContext);
            }
        }

        public IHometaskRepository HometaskRepository
        {
            get
            {
                return _hometaskRepository = _hometaskRepository
                        ?? new HometaskRepository(_applicationContext);
            }
        }


        public Task CommitAsync()
        {
            return _applicationContext.SaveChangesAsync();
        }

        public void Rollback()
        {
            _applicationContext.Dispose();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _applicationContext.Database.BeginTransaction();
        }

        public IStudentGroupRepository StudentGroupRepository
        {
            get
            {
                return _studentGroupRepository = _studentGroupRepository 
                        ?? new StudentGroupRepository(_applicationContext);
            }
        }

    }
}
