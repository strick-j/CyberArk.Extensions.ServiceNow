using CyberArk.Extensions.Plugins.Models;
using CyberArk.Extensions.Utilties.CPMParametersValidation;
using CyberArk.Extensions.Utilties.CPMPluginErrorCodeStandarts;
using CyberArk.Extensions.Utilties.Logger;
using CyberArk.Extensions.Utilties.Reader;
using Newtonsoft.Json;
using CyberArk.Extensions.Plugin.RestAPI;
using CyberArk.Extensions.ServiceNow.Properties;
using System.Security;
using System.Text;

namespace CyberArk.Extensions.ServiceNow
{
    public class Prereconcile : BaseAction
    {
        #region Consts
        public static readonly string USERNAME = "Username";
        public static readonly string ADDRESS = "Address";
        #endregion

        #region constructor
        /// <summary>
        /// Logon Ctor. Do not change anything unless you would like to initialize local class members
        /// The Ctor passes the logger module and the plug-in account's parameters to base.
        /// Do not change Ctor's definition not create another.
        /// <param name="accountList"></param>
        /// <param name="logger"></param>
        public Prereconcile(List<IAccount> accountList, ILogger logger)
            : base(accountList, logger)
        {
        }
        #endregion

        #region Setter
        /// <summary>
        /// Defines the Action name that the class is implementing - Prereconcile
        /// </summary>
        override public CPMAction ActionName
        {
            get { return CPMAction.prereconcilepass; }
        }
        #endregion

        /// <summary>
        /// Plug-in Starting point function.
        /// </summary>
        /// <param name="platformOutput"></param>
        override public int run(ref PlatformOutput platformOutput)
        {
            Logger.MethodStart();
            #region Init
            ErrorCodeStandards errCodeStandards = new ErrorCodeStandards();
            int RC = 9999;
            string empty = string.Empty;
            #endregion

            try
            {
                SetDefaultValues(errCodeStandards, ref empty);
                Logger.WriteLine("Attempting to fetch account properties", LogLevel.INFO);

                #region Fetch Account Properties (FileCategories)
                string username = ParametersAPI.GetMandatoryParameter(USERNAME, TargetAccount.AccountProp);
                ParametersAPI.ValidateParameterLength(username, "Username", 64);

                string address = ParametersAPI.GetMandatoryParameter(ADDRESS, TargetAccount.AccountProp);
                ParametersAPI.ValidateURI(address, "Address", UriKind.RelativeOrAbsolute);

                string reconcileUsername = ParametersAPI.GetMandatoryParameter(USERNAME, ReconcileAccount.AccountProp);
                ParametersAPI.ValidateParameterLength(reconcileUsername, "Username", 64);
                #endregion

                #region Fetch Account's Passwords
                SecureString secureRecPass = ReconcileAccount.CurrentPassword;
                ParametersAPI.ValidatePasswordIsNotEmpty(secureRecPass, "Reconcile Password", 8201);
                #endregion

                #region Logic
                // Generate ServiceNow URI
                // Check ServiceNow Address URI for Scheme, add scheme if not present, add path and convert to string
                Logger.WriteLine(string.Format("Generating ServiceNow URL"), LogLevel.INFO);
                UriBuilder addressBuilder = new(address)
                {
                    Scheme = "https",
                    Port = -1,
                    Path = "/api/now/table/sys_user"
                };
                Uri validatedAddress = addressBuilder.Uri;
                string serviceNowAddress = validatedAddress.ToString();
                Logger.WriteLine(string.Format("Generated ServiceNow URL: {0}", serviceNowAddress), LogLevel.INFO);

                // Generate basic auth header and create auth header string
                // Utilize reconcile username and reconcile password
                Logger.WriteLine("Generating Basic Authorization Header using Reconcile User", LogLevel.INFO);
                var authHeader = Encoding.ASCII.GetBytes($"{reconcileUsername}:{secureRecPass.convertSecureStringToString()}");
                string basicAuthHeader = string.Format("Basic {0}", Convert.ToBase64String(authHeader));
                Logger.WriteLine("Generated Basic Authorization Header", LogLevel.INFO);

                // Generate logon query parameters
                // sysparm_display_value=true&sysparm_limit=1&syspam_fields=sys_id&syparm_query=user_name=<user_name>
                Logger.WriteLine("Generating query parameters for logon", LogLevel.INFO);
                var logonQueryParams = new Dictionary<string, string>
                {
                    {"sysparm_display_value", "true" },
                    {"sysparm_limit", "1" },
                    {"sysparm_fields", "sys_id" },
                    {"sysparm_query" , string.Format("user_name={0}", username)}
                };
                Logger.WriteLine(string.Format("Generated query dictionary"), LogLevel.INFO);

                // Initialize Client 
                JsonSerializer _jsonWriter = new()
                {
                    NullValueHandling = NullValueHandling.Ignore
                };
                var _client = new ServiceNowClient(_jsonWriter);

                // Utilize generated URI, query params, and auth header to retrieve user sys_id
                Logger.WriteLine(string.Format(Resources.SendActionRequest + "sys_id to {0}", serviceNowAddress), LogLevel.INFO);
                var sysIdResponse = _client.GetJsonBody(serviceNowAddress, logonQueryParams, basicAuthHeader);
                if (sysIdResponse.Failure)
                {
                    if (sysIdResponse is HttpErrorResult<Response> httpErrorResult)
                        ServiceNowAPI.HandleHttpErrorResult(httpErrorResult);
                    else if (sysIdResponse is WebExceptionResult<Response> WebExceptionResult)
                        ServiceNowAPI.HandleWebExceptionResult(WebExceptionResult.RequestException);
                    else if (sysIdResponse is ErrorResult<Response> errorResult)
                        ServiceNowAPI.HandleErrorResult(errorResult);
                    else
                        sysIdResponse.MissingPatternMatch();
                }
                else if (sysIdResponse.Success)
                {
                    // Extract sys_id From JSON
                    Logger.WriteLine(string.Format("Extracting sys_id from {0} response.", serviceNowAddress), LogLevel.INFO);
                    SysIdRoot? deserializedResponse = JsonConvert.DeserializeObject<SysIdRoot>(sysIdResponse.Data.MessageContent);
                    // Ensure sys_id is not null, throw error otherwise         
                    if (deserializedResponse?.SysIdResults?[0] == null)
                        throw new ServiceNowServiceException(string.Format(Resources.SysIdError), PluginErrors.JSON_SYSID_ERROR);
                    Logger.WriteLine(Resources.SysIdSuccess, LogLevel.INFO);
                    Logger.WriteLine(string.Format("sys_id " + Resources.ActionResponseSuccess), LogLevel.INFO);
                }

                Logger.WriteLine(Resources.LogonSuccess, LogLevel.INFO);
                RC = 0;
                #endregion Logic
            }
            catch (ParametersConfigurationException ex)
            {
                Logger.WriteLine(string.Format("Recieved exception: {0}", ex.Message), LogLevel.ERROR);
                platformOutput.Message = ex.Message;
                RC = ex.ErrorCode;
            }
            catch (ServiceNowServiceException ex)
            {
                Logger.WriteLine(string.Format("Recieved exception: {0}", ex.Message), LogLevel.ERROR);
                platformOutput.Message = ex.Message;
                RC = ex.ErrorCode;
            }
            catch (Exception ex)
            {
                RC = HandleGeneralError(ex, ref platformOutput);
            }
            finally
            {
                Logger.MethodEnd();
            }

            return RC;
        }
    }
}
