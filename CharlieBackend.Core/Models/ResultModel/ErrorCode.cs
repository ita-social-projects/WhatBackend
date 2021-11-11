namespace CharlieBackend.Core.Models.ResultModel
{
    public enum ErrorCode
    {
        ValidationError,
        Unauthorized,
        InternalServerError,
        NotFound,
        UnprocessableEntity,
        Conflict,
        ForgotPasswordExpired,
        Forbidden,
    }
}

