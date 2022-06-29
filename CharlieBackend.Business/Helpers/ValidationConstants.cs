namespace CharlieBackend.Business.Helpers
{
    public static class ValidationConstants
    {
        public const int MinLength = 8;
        public const int MinLengthName = 2;
        public const int MaxLengthPassword = 30;
        public const int MaxLengthEmail = 50;
        public const int MaxLengthName = 30;
        public const int MaxLengthHeader = 100;
        public const int MaxLengthURL = 200;
        public const int MaxLengthText = 65535; // Length of TEXT type
        public const int MaxLengthCommentText = 1024;
        public const string EmailRegexCheck = @"^([a-zA-Z0-9_-]+\.)*[a-zA-Z0-9_-]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)*\.[a-z]{2,6}$";
        public const string NameRegexCheck = @"^[a-zA-Z '.-]*$";
    }
}
