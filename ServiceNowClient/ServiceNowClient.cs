using CyberArk.Extensions.Plugin.RestAPI;
using CyberArk.Extensions.Utilties.Logger;
using CyberArk.Extensions.ServiceNow.Properties;
using Newtonsoft.Json;
using System.Net;

namespace CyberArk.Extensions.ServiceNow
{
    public class ServiceNowClient : IServiceNowClient
    {
        private readonly JsonSerializer _jsonSerializer;

        public ServiceNowClient(JsonSerializer jsonSerializer)
        {
            _jsonSerializer = jsonSerializer ?? throw new ArgumentNullException(nameof(jsonSerializer));
        }

        public Result<Response> GetJsonBody(string address, Dictionary<string, string> queryParams, string authHeader)
        {
            Logger.MethodStart();
            try
            {
                var response = new Get().Create(address, null, RestAPIConsts.ContentTypes.JSON, queryParams, null, authHeader);
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        if (response.MessageContent == null)
                            return new ErrorResult<Response>("null content response returned");

                        return new SuccessResult<Response>(response);
                    }
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        var responseString = response.MessageContent;
                        return new HttpErrorResult<Response>(responseString, response.StatusCode);
                    }

                    return new ErrorResult<Response>("Unknown Error");
                }
            }
            catch (WebException ex)
            {
                Logger.WriteLine(string.Format("Task " + Resources.ActionResponseFailure + "HttpRequestException Caught."), LogLevel.ERROR);
                return new WebExceptionResult<Response>("Caught WebException", ex);
            }
            finally
            {
                Logger.MethodEnd();
            }
        }

        public Result<Response> PatchJsonBody(string address, string body, Dictionary<string, string> queryParams, string authHeader)
        {
            Logger.MethodStart();
            try
            {
                var response = new Patch().Create(address, body, RestAPIConsts.ContentTypes.JSON, queryParams, null, authHeader);
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        if (response.MessageContent == null)
                            return new ErrorResult<Response>("null content response returned");

                        return new SuccessResult<Response>(response);
                    }
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        var responseString = response.MessageContent;
                        return new HttpErrorResult<Response>(responseString, response.StatusCode);
                    }

                    return new ErrorResult<Response>("Unknown Error");
                }
            }
            catch (WebException ex)
            {
                Logger.WriteLine(string.Format("Task " + Resources.ActionResponseFailure + "HttpRequestException Caught."), LogLevel.ERROR);
                return new WebExceptionResult<Response>("Caught WebException", ex);
            }
            finally
            {
                Logger.MethodEnd();
            }
        }
    }
}