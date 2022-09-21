using IdentityMicroService.Database;
using IdentityMicroService.Modules.Identity;
using IdentityMicroService.Options;

var builder = WebApplication.CreateBuilder(args);

#region services
builder.Services.AddSingleton<IDBConnectionFactory, DBConnectionFactory>();
builder.Services.RegisterIdentityServices();
#endregion services

#region options
builder.Services.AddAuthenticationOptions(builder.Configuration);
builder.Services.AddSwaggerOptions();
#endregion options

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

#region endpoints
app.RegisterIdentityEndpoints();
#endregion endpoints

app.Run();
