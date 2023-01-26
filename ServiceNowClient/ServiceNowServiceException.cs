namespace CyberArk.Extensions.ServiceNow
{
    public class ServiceNowServiceException : Exception
    {
        public int ErrorCode { get; set; }

        public ServiceNowServiceException(string message, int errorCode)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        public ServiceNowServiceException(string message, int errorCode, Exception ex)
            : base(message, ex)
        {
            ErrorCode = errorCode;
        }
    }
}
