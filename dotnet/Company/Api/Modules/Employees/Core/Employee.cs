﻿namespace Api.Modules.Employees.Core
{
    public class Employee
    {
        public int Id { get; set; }
        public string Email { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public DateTime BirthDate { get; set; }
    }
}

