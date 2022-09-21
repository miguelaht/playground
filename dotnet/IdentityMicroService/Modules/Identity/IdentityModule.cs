using IdentityMicroService.Modules.Identity.Adapters;
using IdentityMicroService.Modules.Identity.Contracts;
using IdentityMicroService.Modules.Identity.Ports;

namespace IdentityMicroService.Modules.Identity;

public static class IdentityModule
{
    public static IServiceCollection RegisterIdentityServices(this IServiceCollection services)
    {
        services.AddScoped<IIdentityService, IdentityService>();
        return services;
    }

    public static IEndpointRouteBuilder RegisterIdentityEndpoints(
        this IEndpointRouteBuilder endpoints
    )
    {
        endpoints
            .MapPost(
                "/identity/login",
                async (IIdentityService identity, UserRegistrationRequest request) =>
                {
                    var r = await identity.LoginAsync(request.Email, request.Password);

                    if (!r.Success)
                        return Results.BadRequest(r.Errors);

                    return Results.Ok(r);
                }
            )
            .AllowAnonymous();

        endpoints
            .MapPost(
                "/identity/register",
                async (IIdentityService identity, UserRegistrationRequest request) =>
                {
                    var r = await identity.RegisterAsync(request.Email, request.Password);

                    if (!r.Success)
                        return Results.BadRequest(r.Errors);

                    return Results.Ok(r);
                }
            )
            .AllowAnonymous();

        return endpoints;
    }
}
