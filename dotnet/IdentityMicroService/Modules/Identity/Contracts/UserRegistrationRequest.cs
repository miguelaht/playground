namespace IdentityMicroService.Modules.Identity.Contracts;

public sealed record UserRegistrationRequest(string Email, string Password);
