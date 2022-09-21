using IdentityMicroService.Modules.Identity.Contracts;

namespace IdentityMicroService.Modules.Identity.Ports;

public interface IIdentityService
{
    Task<AuthenticationResult> LoginAsync(string email, string password);
    Task<AuthenticationResult> RegisterAsync(string email, string password);
}
