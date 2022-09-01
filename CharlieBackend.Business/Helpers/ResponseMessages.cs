namespace CharlieBackend.Business.Helpers
{
    public static class ResponseMessages
    {
        public static string NotExist(string entityName) => $"{entityName} {Resources.SharedResources.DoesNotExist}";

        public static string NotValid(string entityName) => $"{entityName} {Resources.SharedResources.IsNotValid}";

        public static string NotActive(string entityName) => $"{entityName} {Resources.SharedResources.IsNotActive}";
    }
}
