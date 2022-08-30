using System;
using Api.Modules.Contracts;
using Api.Modules.Ports;
using FluentValidation;

namespace Api.Modules.Endpoints
{
    public class CreateEmployee
    {
        public async Task<IResult> HandleAsync(
            IEmployeeRepository employees,
            CreateEmployeeRequest req,
            IValidator<CreateEmployeeRequest> validator,
            LinkGenerator linker)
        {
            var validationResult = await validator.ValidateAsync(req);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var emp = await employees.Create(req.ToEmployee());

            if (emp is null)
            {
                return Results.Problem();
            }

            return Results.Created(linker.GetPathByName(nameof(ViewEmployee), values: new { id = emp.Id })!, emp);
        }
    }
}

