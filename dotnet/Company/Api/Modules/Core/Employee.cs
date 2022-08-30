using System;
namespace Api.Modules.Core
{
    public class Employee
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Employee employee &&
                   Id == employee.Id &&
                   Email == employee.Email &&
                   FullName == employee.FullName &&
                   DateOnly.FromDateTime(BirthDate) == DateOnly.FromDateTime(employee.BirthDate);
        }
    }
}

