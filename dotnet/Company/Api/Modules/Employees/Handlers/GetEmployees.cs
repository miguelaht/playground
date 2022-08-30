using Api.Modules.Employees.Contracts;
using Api.Modules.Employees.Ports;
using MediatR;

namespace Api.Modules.Employees.Handlers
{
    public class GetEmployees : IRequestHandler<GetEmployeesRequest, IResult>
    {
        readonly IEmployeeRepository employees;

        public GetEmployees(IEmployeeRepository employees)
        {
            this.employees = employees;
        }
        public async Task<IResult> Handle(GetEmployeesRequest request, CancellationToken cancellationToken)
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

