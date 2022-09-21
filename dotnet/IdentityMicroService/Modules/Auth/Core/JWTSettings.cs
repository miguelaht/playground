namespace IdentityMicroService.Modules.Auth.Core;

public sealed record JWTSettings
{
    public string Secret { get; set; }
}
