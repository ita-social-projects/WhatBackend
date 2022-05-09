using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

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
        private IEventOccurrenceRepository _scheduleRepository;
        private IAttachmentRepository _attachmentRepository;
        private IHomeworkRepository _homeworkRepository;
        private IScheduledEventRepository _scheduledEventRepository;
        private IHomeworkStudentRepository _homeworkStudentRepository;
        private IHomeworkStudentHistoryRepository _homeworkStudentHistoryRepository;
        private IMarkRepository _markRepository;
        private IEventColorRepository _eventColorRepository;

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

        public IEventOccurrenceRepository EventOccurrenceRepository
        {
            get
            {
                return _scheduleRepository = _scheduleRepository
                        ?? new EventOccurrenceRepository(_applicationContext);
            }
        }

        public IHomeworkRepository HomeworkRepository
        {
            get
            {
                return _homeworkRepository = _homeworkRepository
                        ?? new HomeworkRepository(_applicationContext);
            }
        }

        public IScheduledEventRepository ScheduledEventRepository 
        {
            get
            {
                return _scheduledEventRepository = _scheduledEventRepository
                        ?? new ScheduledEventRepository(_applicationContext);
            } 
        }
        public IHomeworkStudentRepository HomeworkStudentRepository
        {
            get 
            {
                return _homeworkStudentRepository = _homeworkStudentRepository
                        ?? new HomeworkStudentRepositrory(_applicationContext);
            }
        }

        public IHomeworkStudentHistoryRepository HomeworkStudentHistoryRepository
        {
            get
            {
                return _homeworkStudentHistoryRepository = _homeworkStudentHistoryRepository
                        ?? new HomeworkStudentHistoryRepositrory(_applicationContext);
            }
        }

        public IMarkRepository MarkRepository
        {
            get
            {
                return _markRepository = _markRepository
                        ?? new MarkRepository(_applicationContext);
            }
        }

        public IEventColorRepository EventColorRepository 
        { 
            get
            {
                return _eventColorRepository = _eventColorRepository 
                    ?? new EventColorRepository(_applicationContext);
            }
        }

        public async Task CommitAsync()
        {
            await _applicationContext.SaveChangesAsync();
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
