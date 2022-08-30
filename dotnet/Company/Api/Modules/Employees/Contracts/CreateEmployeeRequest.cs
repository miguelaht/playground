using Api.Modules.Employees.Core;
using MediatR;

namespace Api.Modules.Employees.Contracts
{
    public class CreateEmployeeRequest: IRequest<IResult>
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }

        public Employee ToEmployee()
        {
            return new Employee { Email = this.Email, FullName = this.FullName, BirthDate = this.BirthDate };
        }
    }
}

