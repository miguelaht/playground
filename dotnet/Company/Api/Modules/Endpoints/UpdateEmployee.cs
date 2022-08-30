using System;
using Api.Modules.Contracts;
using Api.Modules.Ports;
using FluentValidation;

namespace Api.Modules.Endpoints
{
    public class UpdateEmployee
    {
        public async Task<IResult> HandleAsync(
            IEmployeeRepository employees,
            UpdateEmployeeRequest req,
            IValidator<UpdateEmployeeRequest> validator,
            int id)
        {
            var validationResult = await validator.ValidateAsync(req);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var emp = await employees.View(id);

            if (emp is null || req.Id != id)
            {
                return Results.NotFound();
            }

            var updatedEmp = await employees.Update(req.ToEmployee());

            if (updatedEmp is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(updatedEmp);
        }
    }
}

