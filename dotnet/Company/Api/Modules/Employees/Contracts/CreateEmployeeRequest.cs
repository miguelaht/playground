using Api.Modules.Employees.Core;
using MediatR;

namespace Api.Modules.Employees.Contracts
{
    public class CreateEmployeeRequest : IRequest<IResult>
    {
        public string Email { get; init; } = default!;
        public string FullName { get; init; } = default!;
        public DateTime BirthDate { get; init; }

        public Employee ToEmployee()
        {
            return new Employee { Email = this.Email, FullName = this.FullName, BirthDate = this.BirthDate };
        }
    }
}

