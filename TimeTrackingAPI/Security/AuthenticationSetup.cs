using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TimeTrackingAPI.Security;

public static class AuthenticationSetup
{
    public static IServiceCollection AddRequiredBearerAuthentication(this IServiceCollection services, IConfiguration config)
    {
        var authority = config["Authentication:Authority"];
        var audience = config["Authentication:Audience"];
        if (!Uri.TryCreate(authority, UriKind.Absolute, out var uri) || uri.Scheme != "https" || string.IsNullOrWhiteSpace(audience))
            throw new InvalidOperationException("Configure Authentication:Authority (HTTPS OIDC issuer) and Authentication:Audience.");
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.Authority = authority;
            options.Audience = audience;
            options.RequireHttpsMetadata = true;
            options.MapInboundClaims = false;
            options.TokenValidationParameters.ClockSkew = TimeSpan.FromSeconds(30);
            options.TokenValidationParameters.RoleClaimType = config["Authentication:RoleClaimType"] ?? "roles";
        });
        services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            options.AddPolicy("ApiWrite", policy => policy.RequireAuthenticatedUser().RequireRole("ApiWriter"));
        });
        return services;
    }
}
