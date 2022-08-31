using Api.Modules.Employees.Adapters;
using Api.Modules.Employees.Contracts;
using Api.Modules.Employees.Core;
using Api.Modules.Employees.Handlers;
using Api.Modules.Employees.Ports;
using Api.Modules.Employees.Validators;
using FluentValidation;
using MediatR;

namespace Api.Modules.Employees
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
            endpoints.MapPost("/employees", (IMediator mediator, CreateEmployeeRequest request)
                    => mediator.Send(request))
                .WithTags("Employees")
                .WithName(nameof(CreateEmployee))
                .Produces<Employee>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status500InternalServerError)
                .ProducesProblem(StatusCodes.Status400BadRequest);

            endpoints.MapGet("/employees/{id:int}", (IMediator mediator, int id)
                    => mediator.Send(new ViewEmployeeRequest { Id = id }))
                .WithTags("Employees")
                .WithName(nameof(ViewEmployee))
                .Produces<Employee>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound);

            endpoints.MapGet("/employees", (IMediator mediator)
                    => mediator.Send(new GetEmployeesRequest()))
                .WithTags("Employees")
                .WithName(nameof(GetEmployees))
                .Produces<Employee>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound);

            endpoints.MapDelete("/employees/{id:int}", (IMediator mediator, int id)
                    => mediator.Send(new DeleteEmployeeRequest { Id = id }))
                .WithTags("Employees")
                .WithName(nameof(DeleteEmployee))
                .Produces(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound);

            endpoints.MapPut("/employees/{id:int}", (IMediator mediator, int id, UpdateEmployeeRequest request)
                    =>
            {
                if (id != request.Id) return Results.NotFound();
                return mediator.Send(request).Result;
            })
                .WithTags("Employees")
                .WithName(nameof(UpdateEmployee))
                .Produces<Employee>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound);

            return endpoints;
        }
    }
}

