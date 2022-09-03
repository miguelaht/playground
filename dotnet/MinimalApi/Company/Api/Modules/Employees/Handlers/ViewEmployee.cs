using Api.Modules.Employees.Contracts;
using Api.Modules.Employees.Ports;
using MediatR;

namespace Api.Modules.Employees.Handlers
{
    public class ViewEmployee : IRequestHandler<ViewEmployeeRequest, IResult>
    {
        private readonly IEmployeeRepository employees;

        public ViewEmployee(IEmployeeRepository employees)
        {
            this.employees = employees;
        }

        public async Task<IResult> Handle(
            ViewEmployeeRequest request,
            CancellationToken cancellationToken
        )
        {
            var emp = await employees.View(request.Id);

            if (emp is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(emp);
        }
    }
}
