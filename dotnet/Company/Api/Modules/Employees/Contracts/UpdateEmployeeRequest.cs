using Api.Modules.Employees.Core;
using MediatR;

namespace Api.Modules.Employees.Contracts
{
    public class UpdateEmployeeRequest : IRequest<IResult>
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }

        public Employee ToEmployee()
        {
            return new Employee { Id = this.Id, Email = this.Email, FullName = this.FullName, BirthDate = this.BirthDate };
        }
    }
}

