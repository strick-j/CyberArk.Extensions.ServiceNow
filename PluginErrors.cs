namespace CyberArk.Extensions.ServiceNow
{
    /*
     * Plug-in Errors class should contain common error numbers and messages for all operations.
     * Specific action error messages should implemented in the action class itself 
     */
    public static class PluginErrors
    {
        // STANDARD_DEFUALT_ERROR_CODE_IDX --> index for standard error code.
        public static readonly int STANDARD_DEFUALT_ERROR_CODE_IDX = 9999;

        public static readonly int NO_SUCH_ENTITY_ERROR_NUMBER = 8800;
        public static readonly int MANDATORY_PARAMETER_MISSING = 8801;
        public static readonly int JSON_SYSID_ERROR = 8802;
        public static readonly int JSON_CHANGE_ERROR = 8803;
        public static readonly int JSON_REC_ERROR = 8804;
        public static readonly int RESPONSE_CONTENT_NULL = 8805;
        public static readonly int RESPONSE_GENERIC_EXCEPTION = 8506;
        public static readonly int RESPONSE_TIMEOUT_EXCEPTION = 8507;

        // SUCCESS_ERROR --> Errors when 200 Success is returned but the action failed.
        public static readonly int SUCCESS_DEFAULT_ERROR = 8810;
        public static readonly int SUCCESS_USER_NOT_FOUND = 8811;
        public static readonly int SUCCESS_USER_ID_NOT_FOUND = 8812;
        public static readonly int SUCCESS_USER_NOT_AUTHORIZED = 8813;
        public static readonly int SUCCESS_SERVICE_NOT_AUTHORIZED = 8814;
        public static readonly int SUCCESS_INVALID_PASSWORD_COMPLEXITY = 8815;
        public static readonly int SUCCESS_PASSWORD_SAME_AS_OLD = 8816;
        public static readonly int SUCCESS_PASSWORD_LAST_N = 8817;
        public static readonly int SUCCESS_JSON_EXCEPTION = 8818;
        public static readonly int SUCCESS_REQUIRED_PARAMETER = 8819;

        // BAD_REQUEST_ERRORS --> Errors returned with 400 Bad Request Response.
        public static readonly int BAD_REQUEST_NULL_RESPONSE = 8820;
        public static readonly int BAD_REQUEST_UNHANDLED = 8821;
        public static readonly int BAD_REQUEST_INVALID_RESOURCE = 8822;

        // STATUS_CODE_ERROS --> Errors related to non success status code.
        public static readonly int STATUS_CODE_UNAUTHORIZED = 8830;
        public static readonly int STATUS_CODE_FORBIDDEN = 8831;
        public static readonly int STATUS_CODE_NOT_FOUND = 8832;
        public static readonly int STATUS_CODE_METHOD_NOT_ALLOWED = 8833;
        public static readonly int STATUS_CODE_NOT_ACCEPTABLE = 8834;
        public static readonly int STATUS_CODE_REQ_TIMEOUT = 8835;
        public static readonly int STATUS_CODE_GW_TIMEOUT = 8836;
        public static readonly int STATUS_CODE_PROXY_AUTH = 8837;
        public static readonly int STATUS_CODE_UNSUPPORTED_MEDIA_TYPE = 8838;
        public static readonly int STATUS_CODE_UNHANDLED = 8839;

        // WEB_ERRORS --> Errors received with HTTP Transport, no response recieved.
        public static readonly int WEB_GENERIC_EXCEPTION = 8840;
        public static readonly int WEB_NAME_RESOLUTION_FAILURE = 8841;
        public static readonly int WEB_CONNECT_FAILURE = 8842;
        public static readonly int WEB_UNHANDLED = 8843;
        public static readonly int WEB_SSL_TLS_EXCEPTION = 8844;

        public static readonly int DEFAULT_ERROR_NUMBER = -1;
    }
}