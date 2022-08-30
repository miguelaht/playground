using System;
using Api.Modules.Adapters;
using Api.Modules.Contracts;
using Api.Modules.Core;
using Api.Modules.Endpoints;
using Api.Modules.Ports;
using Api.Modules.Validators;
using FluentValidation;

namespace Api.Modules
{
    public static class EmployeesModule
    {
        public static IServiceCollection RegisterEmployeeServices(this IServiceCollection services)
        {
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            services.AddScoped<IValidator<CreateEmployeeRequest>, CreateEmployeeValidator>();
            services.AddScoped<IValidator<UpdateEmployeeRequest>, UpdateEmployeeValidator>();

            return services;
        }

        public static IEndpointRouteBuilder RegisterEmployeeEndpoints(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost("/employees", new CreateEmployee().HandleAsync)
                .WithTags("Employees")
                .WithName(nameof(CreateEmployee))
                .Produces<Employee>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status500InternalServerError)
                .ProducesProblem(StatusCodes.Status400BadRequest);

            endpoints.MapGet("/employees/{id:int}", new ViewEmployee().HandleAsync)
                .WithTags("Employees")
                .WithName(nameof(ViewEmployee))
                .Produces<Employee>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound);

            endpoints.MapGet("/employees", new GetEmployees().HandleAsync)
                .WithTags("Employees")
                .WithName(nameof(GetEmployees))
                .Produces<Employee>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound);

            endpoints.MapDelete("/employees/{id:int}", new DeleteEmployee().HandleAsync)
                .WithTags("Employees")
                .WithName(nameof(DeleteEmployee))
                .Produces<Employee>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound);

            endpoints.MapPut("/employees/{id:int}", new UpdateEmployee().HandleAsync)
                .WithTags("Employees")
                .WithName(nameof(UpdateEmployee))
                .Produces<Employee>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound);

            return endpoints;
        }
    }
}

