using System;
using MeMetrics.Api.Middleware.Authentication;
using MeMetrics.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MeMetrics.Api.Configurations.Extensions
{
    public static class AuthenticationConfigurationExtensions
    {
        internal static void AddApiAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var environmentConfiguration = new EnvironmentConfiguration();
            configuration.Bind(environmentConfiguration);

            services.AddAuthentication()
                .AddApiKey(options =>
                {
                    options.AuthKey = new string[] { environmentConfiguration.PRIMARY_API_KEY, environmentConfiguration.SECONDARY_API_KEY };
                });
            var policy = new AuthorizationPolicyBuilder {AuthenticationSchemes = new string[] {ApiKeyOptions.DefaultScheme}};
            policy.RequireAuthenticatedUser();
            services.AddAuthorization(o => o.DefaultPolicy = policy.Build());
        }

        internal static AuthenticationBuilder AddApiKey(this AuthenticationBuilder builder, Action<ApiKeyOptions> options)
        {
            return builder.AddScheme<ApiKeyOptions, ApiKeyHandler>(ApiKeyOptions.DefaultScheme, options);
        }
    }
}
