namespace CharlieBackend.Business.Helpers
{
    public static class ResponseMessages
    {
        public const string GroupNotFound = "Group not found";

        public const string GroupHasNotStudents = "Group hasn't any students";

        public const string IndexNotValid = "Index cannot be less then 1 and greater than 5";

        public static string NotExist(string entityName) => $"{entityName} does not exist ";

        public static string NotValid(string entityName) => $"{entityName} is not valid ";

        public static string NotActive(string entityName) => $"{entityName} is not active ";
    }
}
