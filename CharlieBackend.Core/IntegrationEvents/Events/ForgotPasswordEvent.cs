using Newtonsoft.Json;
using CharlieBackend.Core.Entities;

namespace CharlieBackend.Core.IntegrationEvents.Events
{
    public class ForgotPasswordEvent
    {
        [JsonConstructor]
        public ForgotPasswordEvent(string recepientMail, string url)
        {
            RecepientMail = recepientMail;
            Url = url;
        }

        public string RecepientMail { get; private set; }

        public string Url { get; private set; }
    }
}
