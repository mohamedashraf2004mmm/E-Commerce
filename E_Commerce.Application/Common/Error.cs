namespace E_Commerce.Application.Common
{
    public record Error(string code , string Description , ErrorType ErrorType = ErrorType.Failure)
    {
        public static Error Failure(string code = "General.Failure" , string description = "General failure has occured")
        {
            return new Error(code, description, ErrorType.Failure);
        }

        public static Error Validation(string code = "General.Validation", string description = "General Validation has occured")
        {
            return new Error(code, description, ErrorType.Validation);
        }

        public static Error NotFound(string code = "General.NotFound", string description = "Resource not found")
        {
            return new Error(code, description, ErrorType.NotFound);
        }

        public static Error Conflict(string code = "General.Conflict", string description = "General Conflict has occured")
        {
            return new Error(code, description, ErrorType.Conflict);
        }

        public static Error Unauthorized(string code = "General.Unauthorized", string description = "UnAuthorized!!!")
        {
            return new Error(code, description, ErrorType.Unauthorized);
        }

        public static Error Forbidden(string code = "General.Forbidden", string description = "Forbidden!!!")
        {
            return new Error(code, description, ErrorType.Forbidden);
        }

        public static Error InvalidCredentials(string code = "General.InvalidCredentials", string description = "InvalidCredentials!!!")
        {
            return new Error(code, description, ErrorType.InvalidCredentials);
        }
    }

    public enum ErrorType
    {
        Failure = 0,
        Validation = 1 ,
        NotFound = 2,
        Conflict = 3,
        Unauthorized = 4,
        Forbidden = 5,
        InvalidCredentials = 6
    }
}