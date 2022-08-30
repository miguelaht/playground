using System;
using Api.Database;
using Api.Modules.Core;
using Api.Modules.Ports;
using Dapper;

namespace Api.Modules.Adapters
{
    public class EmployeeRepository: IEmployeeRepository
    {
        private readonly IDBConnectionFactory _conn;

        public EmployeeRepository(IDBConnectionFactory conn)
        {
            _conn = conn;
        }

        public async Task<Employee> Create(Employee emp)
        {
            using (var connection = await _conn.CreateConnectionAsync())
            {
                var query = @"INSERT INTO Employees
                                (FullName, BirthDate, Email)
                                OUTPUT INSERTED.Id
                                VALUES (@fullName, @birthDate, @email)";
                var parameters = new DynamicParameters();
                parameters.Add("@fullName", emp.FullName);
                parameters.Add("@birthDate", emp.BirthDate);
                parameters.Add("@email", emp.Email);

                var newId = await connection.QuerySingleAsync<int>(query, parameters);

                emp.Id = newId;

                return emp;
            }
        }

        public async Task<bool> Delete(int id)
        {
            using (var connection = await _conn.CreateConnectionAsync())
            {
                var query = @"DELETE FROM Employees WHERE Id=@id";
                var parameters = new DynamicParameters();
                parameters.Add("@id", id);

                var affectedRows = await connection.ExecuteAsync(query, parameters);

                return affectedRows != 0;
            }
        }

        public async Task<IEnumerable<Employee>> GetAll()
        {
            using (var connection = await _conn.CreateConnectionAsync())
            {
                var query = @"SELECT Id, FullName, BirthDate, Email FROM Employees";

                var employees = await connection.QueryAsync<Employee>(query);

                return employees;
            }
        }

        public async Task<Employee?> Update(Employee emp)
        {
            using (var connection = await _conn.CreateConnectionAsync())
            {
                var query = @"UPDATE Employees
                                SET
                                    FullName=@fullName,
                                    BirthDate=@birthDate,
                                    Email=@email
                                WHERE Id=@id";
                var parameters = new DynamicParameters();
                parameters.Add("@fullName", emp.FullName);
                parameters.Add("@birthDate", emp.BirthDate);
                parameters.Add("@email", emp.Email);
                parameters.Add("@id", emp.Id);

                var affectedRows = await connection.ExecuteAsync(query, parameters);

                return affectedRows != 0 ? emp : null;
            }
        }

        public async Task<Employee?> View(int id)
        {
            using (var connection = await _conn.CreateConnectionAsync())
            {
                var query = @"SELECT Id, FullName, BirthDate, Email
                                FROM Employees
                                WHERE Id=@id";
                var parameters = new DynamicParameters();
                parameters.Add("@id", id);

                var employee = await connection.QueryFirstOrDefaultAsync<Employee>(query, parameters);

                return employee;
            }
        }

        public async Task<Employee?> ViewByEmail(string email)
        {
            using (var connection = await _conn.CreateConnectionAsync())
            {
                var query = @"SELECT Id, FullName, BirthDate, Email
                                FROM Employees
                                WHERE Email=@email";
                var parameters = new DynamicParameters();
                parameters.Add("@email", email);

                var employee = await connection.QueryFirstOrDefaultAsync<Employee>(query, parameters);

                return employee;
            }
        }
    }
}

