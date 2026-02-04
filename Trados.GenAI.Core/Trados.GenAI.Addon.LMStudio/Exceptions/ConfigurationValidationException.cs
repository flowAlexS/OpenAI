using System.Net;

namespace Trados.GenAI.Addon.LMStudio.Exceptions
{
    public class ConfigurationValidationException : AppException
    {
        public ConfigurationValidationException(string message, Details[] details) : base(message)
        {
            ErrorCode = ErrorCodes.InvalidConfiguration;
            StatusCode = HttpStatusCode.BadRequest;
            ExceptionDetails = details;
        }
    }
}
