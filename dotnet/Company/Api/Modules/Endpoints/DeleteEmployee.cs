using System;
using Api.Modules.Ports;

namespace Api.Modules.Endpoints
{
    public class DeleteEmployee
    {
        public async Task<IResult> HandleAsync(
            IEmployeeRepository employees,
            int id)
        {
            var emp = await employees.Delete(id);

            if (!emp)
            {
                return Results.NotFound();
            }

            return Results.Ok(emp);
        }
    }
}

