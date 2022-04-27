using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Business.Helpers
{
    public class ResponseMessages
    {
        public const string GroupNotFound = "Group not found";

        public const string GroupHasNotStudents = "Group hasn't any students";

        public const string IndexNotValid = "Index cannot be less then 1 and greater than 5";

        public static string NotExist(string entityName) => $"{entityName} does not exist\t";
    }
}
