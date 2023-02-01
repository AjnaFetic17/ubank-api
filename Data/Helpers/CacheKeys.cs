using Microsoft.AspNetCore.Http;

namespace ubank_api.Data.Helpers
{
    internal static class CacheKeys
    {
        internal const string Employee = "EmployeeCachKey";
        internal const string Client = "ClientCachKey";
        internal const string Services = "ServicesCachKey"; 
        internal const string SasToken = "SasTokenCachKey";
        internal const string User = "UserCachKey";
    }
}
