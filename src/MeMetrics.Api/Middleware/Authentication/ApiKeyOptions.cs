using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Primitives;

namespace MeMetrics.Api.Middleware.Authentication
{
    public class ApiKeyOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "SecretKey";
        public string Scheme => DefaultScheme;
        public StringValues AuthKey { get; set; }
    }
}
