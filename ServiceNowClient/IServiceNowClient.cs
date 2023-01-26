using Newtonsoft.Json;
using CyberArk.Extensions.Plugin.RestAPI;

namespace CyberArk.Extensions.ServiceNow
{
    public interface IServiceNowClient
    {
        Result<Response> GetJsonBody(string address, Dictionary<string, string> queryParams, string authHeader);
        Result<Response> PatchJsonBody(string address, string body, Dictionary<string, string> queryParams, string authToken);
    }

    public class SetPassword
    {
        [JsonProperty("user_password")]
        public string? NewPassword { get; set; }
    }

    public class SysIdResult
    {
        [JsonProperty("sys_id")]
        public string? SysId { get; set; }
    }

    public class SysIdRoot
    {
        [JsonProperty("result")]
        public List<SysIdResult> SysIdResults;
    }

    public class UserPassResult
    {
        [JsonProperty("user_password")]
        public string? UserPassword { get; set; }
    }

    public class UserPassRoot
    {
        [JsonProperty("result")]
        public UserPassResult UserPassResults;
    }
}
