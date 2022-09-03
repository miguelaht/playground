using Api.Modules.Employees.Contracts;
using Api.Modules.Employees.Ports;
using MediatR;

namespace Api.Modules.Employees.Handlers
{
    public class DeleteEmployee : IRequestHandler<DeleteEmployeeRequest, IResult>
    {
        private readonly IEmployeeRepository employees;

        public DeleteEmployee(IEmployeeRepository employees)
        {
            this.employees = employees;
        }

        public async Task<IResult> Handle(
            DeleteEmployeeRequest request,
            CancellationToken cancellationToken
        )
        {
            var emp = await employees.Delete(request.Id);

            if (!emp)
            {
                return Results.NotFound();
            }

            return Results.Ok(emp);
        }
    }
}
