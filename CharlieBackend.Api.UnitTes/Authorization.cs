using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CharlieBackend.Api.UnitTes
{
    [TestClass]
    public class Auth
    {
        [TestMethod]
        public void Authorization()
        {
            string _login = "admin";
            string _pass = "admin";

            


            //Data.ApplicationContext _applicationContext = new Data.ApplicationContext();

            //Data.Repositories.Impl.UnitOfWork _unitOfWork = new Data.Repositories.Impl.UnitOfWork(_applicationContext);
            //Data.Repositories.Impl.AccountRepository _accountRepository = new Data.Repositories.Impl.AccountRepository(_applicationContext); 

            //Business.Services.AccountService _accountService = new Business.Services.AccountService(_unitOfWork);
            //Business.Services.EmailCredentialsSenderService _senderService = new Business.Services.EmailCredentialsSenderService(_login, _pass);
            //Business.Services.StudentService _studentService = new Business.Services.StudentService(_accountService, _unitOfWork, _senderService);
            //Business.Services.MentorService _mentorService = new Business.Services.MentorService(_accountService, _unitOfWork, _senderService);

            //Controllers.AccountsController _accountsController = new Controllers.AccountsController(_accountService, _studentService, _mentorService);
            

        }
    }
}
