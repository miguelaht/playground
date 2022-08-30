using System;
using Api.Modules.Contracts;
using Api.Modules.Ports;
using FluentValidation;

namespace Api.Modules.Endpoints
{
    public class ViewEmployee
    {
        public async Task<IResult> HandleAsync(
            IEmployeeRepository employees,
            int id)
        {
            var emp = await employees.View(id);

            if (emp is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(emp);
        }
    }
}

