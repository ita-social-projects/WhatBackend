namespace CharlieBackend.Business.Helpers
{
    public static class ResponseMessages
    {
        public static string NotExist(string entityName) => $"{entityName} does not exist ";

        public static string NotValid(string entityName) => $"{entityName} is not valid ";

        public static string NotActive(string entityName) => $"{entityName} is not active ";
    }
}
