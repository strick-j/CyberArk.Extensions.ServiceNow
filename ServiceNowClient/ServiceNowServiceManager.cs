using CyberArk.Extensions.Utilties.Logger;
using CyberArk.Extensions.Plugin.RestAPI;
using CyberArk.Extensions.ServiceNow.Properties;
using System.Net;

namespace CyberArk.Extensions.ServiceNow
{
    public class ServiceNowServiceManager
    {
        public void HandleHttpErrorResult(HttpErrorResult<Response> httpErrorResult)
        {
            // Check 400 Bad Request Status Code
            if (httpErrorResult.StatusCode == HttpStatusCode.BadRequest)
            {
                // Check for various reasons 400 Bad Request may be returned
                Logger.WriteLine(Resources.BadRequestRecieved, LogLevel.ERROR);
                if (httpErrorResult.Message.Contains("User Not Authenticated"))
                    throw new ServiceNowServiceException(string.Format(Resources.BadRequestInvalidResource), PluginErrors.BAD_REQUEST_INVALID_RESOURCE);
                else
                    throw new ServiceNowServiceException(string.Format("Bad Request: {0}", httpErrorResult.Message), PluginErrors.BAD_REQUEST_UNHANDLED);
            };

            // Check other common Status Codes
            throw httpErrorResult.StatusCode switch
            {
                HttpStatusCode.Unauthorized => new ServiceNowServiceException("Status Code 401 - Unauthorized", PluginErrors.STATUS_CODE_UNAUTHORIZED),
                HttpStatusCode.Forbidden => new ServiceNowServiceException("Status Code 403 - Forbidden", PluginErrors.STATUS_CODE_FORBIDDEN),
                HttpStatusCode.NotFound => new ServiceNowServiceException("Status Code 404 - Not Found", PluginErrors.STATUS_CODE_NOT_FOUND),
                HttpStatusCode.MethodNotAllowed => new ServiceNowServiceException("Status Code 405 - Method not allowed", PluginErrors.STATUS_CODE_METHOD_NOT_ALLOWED),
                HttpStatusCode.NotAcceptable => new ServiceNowServiceException("Status Code 406 - Not Acceptable", PluginErrors.STATUS_CODE_NOT_ACCEPTABLE),
                HttpStatusCode.ProxyAuthenticationRequired => new ServiceNowServiceException("Status Code 407 - Proxy Authentication Required", PluginErrors.STATUS_CODE_PROXY_AUTH),
                HttpStatusCode.RequestTimeout => new ServiceNowServiceException("Status Code 408 - Request Timeout", PluginErrors.STATUS_CODE_REQ_TIMEOUT),
                HttpStatusCode.UnsupportedMediaType => new ServiceNowServiceException("Status Code 415 - Unsupported Media Type", PluginErrors.STATUS_CODE_UNSUPPORTED_MEDIA_TYPE),
                HttpStatusCode.GatewayTimeout => new ServiceNowServiceException("Status Code 504 - Gateway Timeout", PluginErrors.STATUS_CODE_GW_TIMEOUT),
                _ => new ServiceNowServiceException(Resources.GenericError, PluginErrors.STANDARD_DEFUALT_ERROR_CODE_IDX),
            };
        }

        public void HandleWebExceptionResult(WebException ex)
        {
            throw ex.Status switch
            {
                WebExceptionStatus.NameResolutionFailure => new ServiceNowServiceException(string.Format(Resources.HttpNameResolutionFailure), PluginErrors.WEB_NAME_RESOLUTION_FAILURE),
                WebExceptionStatus.ConnectFailure => new ServiceNowServiceException(string.Format(Resources.HttpConnectFailure), PluginErrors.WEB_CONNECT_FAILURE),
                WebExceptionStatus.SecureChannelFailure => new ServiceNowServiceException(string.Format(Resources.HttpSecureChannelError), PluginErrors.WEB_SSL_TLS_EXCEPTION),
                _ => new ServiceNowServiceException(string.Format(Resources.UnhandledWebException + "{0}", ex.Message), PluginErrors.WEB_UNHANDLED),
            };
        }

        public void HandleErrorResult(ErrorResult<Response> errorResult)
        {
            throw new ServiceNowServiceException(string.Format(Resources.GenericError + "{0}", errorResult.Data.MessageContent), PluginErrors.DEFAULT_ERROR_NUMBER);
        }
    }
}
