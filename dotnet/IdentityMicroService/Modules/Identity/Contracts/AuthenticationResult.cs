namespace IdentityMicroService.Modules.Identity.Contracts;

public sealed record AuthenticationResult
{
    public string Token { get; set; }
    public bool Success { get; set; }
    public IEnumerable<string> Errors { get; set; }
}
