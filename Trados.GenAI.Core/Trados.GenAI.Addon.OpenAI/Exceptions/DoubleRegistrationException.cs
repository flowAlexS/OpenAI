using System.Net;

namespace Trados.GenAI.Addon.OpenAI.Exceptions
{
    public class DoubleRegistrationException : AppException
    {
        public DoubleRegistrationException() : base("The App has been already registered in Language Cloud")
        {
            ErrorCode = ErrorCodes.AlreadyRegistered;
            StatusCode = HttpStatusCode.Conflict;
        }
    }
}
