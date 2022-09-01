using Api.Database;
using Api.Modules.Employees;
using MediatR;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IDBConnectionFactory>(provider =>
        new DBConnectionFactory(builder.Configuration, "main")
    );
builder.Services.AddMediatR(x => x.AsScoped(), typeof(Program));

builder.Services.RegisterEmployeeServices();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.RegisterEmployeeEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();

public partial class Program { }
