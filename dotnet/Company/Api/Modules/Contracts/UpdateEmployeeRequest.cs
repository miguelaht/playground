using System;
using Api.Modules.Core;

namespace Api.Modules.Contracts
{
    public class UpdateEmployeeRequest
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

