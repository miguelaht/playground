namespace IdentityMicroService.Modules.Identity.Contracts;

public sealed record AuthFailedResponse(IEnumerable<string> Errors);
