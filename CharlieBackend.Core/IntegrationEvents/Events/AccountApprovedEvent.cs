using System;
using Newtonsoft.Json;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.IntegrationEvents.Abstractions;

namespace CharlieBackend.Core.IntegrationEvents.Events
{
    //immutable
    public class AccountApprovedEvent : IEvent
    {

        [JsonConstructor]
        public AccountApprovedEvent(string recepientMail, string firstName,
                                   string lastName, UserRole role)
        {
            RecepientMail = recepientMail;
            FirstName = firstName;
            LastName = lastName;
            Role = role.ToString();
        }

        public string RecepientMail { get; private set; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string Role { get; private set; }
    }
}
