using Api.Modules.Employees.Core;

namespace Api.Modules.Employees.Ports
{
    public interface IEmployeeRepository
    {
        Task<Employee> Create(Employee emp);
        Task<Employee?> Update(Employee emp);
        Task<Employee?> View(int id);
        Task<Employee?> ViewByEmail(string email);
        Task<IEnumerable<Employee>> GetAll();
        Task<bool> Delete(int id);
    }
}

