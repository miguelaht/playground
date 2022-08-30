using System;
using Api.Modules.Ports;

namespace Api.Modules.Endpoints
{
    public class GetEmployees
    {
        public async Task<IResult> HandleAsync(
            IEmployeeRepository employees)
        {
            var emps = await employees.GetAll();

            if (emps is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(emps);
        }
    }
}

