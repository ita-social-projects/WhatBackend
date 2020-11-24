using Newtonsoft.Json;

namespace CharlieBackend.Core.IntegrationEvents.Events
{
    //immutable
    public class RegistrationSuccessEvent
    {
        [JsonConstructor]
        public RegistrationSuccessEvent(string recepientMail, 
                                        string firstName,
                                        string lastName)
        {
            RecepientMail = recepientMail;
            FirstName = firstName;
            LastName = lastName;
        }

        public string RecepientMail { get; private set; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }
    }
}
