using Api.Modules.Employees.Contracts;
using Api.Modules.Employees.Core;
using Bogus;
using System.Net.Http.Json;
using System.Net;

namespace Tests;

public class TestEmployeeEndpoints
{
    private readonly AppEndpointTest app;
    private readonly HttpClient client;
    private readonly Faker<CreateEmployeeRequest> generator;

    public TestEmployeeEndpoints()
    {
        app = new();
        client = app.CreateClient();

        generator = new();
        generator.RuleFor(e => e.FullName, f => f.Person.FullName);
        generator.RuleFor(e => e.Email, f => f.Person.Email);
        generator.RuleFor(e => e.BirthDate, f => f.Person.DateOfBirth);
    }

    [Fact]
    public async Task TestCreateEmployee_ValidData()
    {
        var newEmployee = generator.Generate();

        var response = await client.PostAsJsonAsync("/employees", newEmployee);
        var createdEmployee = await response.Content.ReadFromJsonAsync<Employee>();

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(createdEmployee);
        Assert.Equal(newEmployee.FullName, createdEmployee!.FullName);
        Assert.Equal(newEmployee.Email, createdEmployee.Email);
        Assert.Equal(newEmployee.BirthDate.ToString(), createdEmployee.BirthDate.ToString());
    }

    [Fact]
    public async Task TestCreateEmployee_NotValidData()
    {
        var newEmp = new CreateEmployeeRequest { FullName = "asd", Email = "asd", BirthDate = DateTime.Now };
        var response = await client.PostAsJsonAsync("/employees", newEmp);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task TestViewEmployee_ValidId()
    {
        var newEmployee = generator.Generate();

        var response = await client.PostAsJsonAsync("/employees", newEmployee);
        var createdEmployee = await response.Content.ReadFromJsonAsync<Employee>();

        var location = response!.Headers!.Location!.OriginalString;
        response = await client.GetAsync(location);
        var viewEmployee = await response.Content.ReadFromJsonAsync<Employee>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(createdEmployee!.FullName, viewEmployee!.FullName);
        Assert.Equal(createdEmployee.Id, viewEmployee.Id);
        Assert.Equal(createdEmployee.Email, viewEmployee.Email);
        Assert.Equal(DateOnly.FromDateTime(createdEmployee.BirthDate), DateOnly.FromDateTime(viewEmployee.BirthDate));
    }

    [Fact]
    public async Task TestViewEmployee_NotValidId()
    {
        var newEmployee = generator.Generate();

        var response = await client.PostAsJsonAsync("/employees", newEmployee);
        var createdEmployee = await response.Content.ReadFromJsonAsync<Employee>();

        var location = response!.Headers!.Location!.OriginalString;
        response = await client.DeleteAsync(location);
        response = await client.GetAsync(location);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task TestDeleteEmployee_ValidId()
    {
        var newEmployee = generator.Generate();

        var response = await client.PostAsJsonAsync("/employees", newEmployee);
        var createdEmployee = await response.Content.ReadFromJsonAsync<Employee>();

        var location = response!.Headers!.Location!.OriginalString;
        response = await client.DeleteAsync(location);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task TestDeleteEmployee_NotValidId()
    {
        var newEmployee = generator.Generate();

        var response = await client.PostAsJsonAsync("/employees", newEmployee);
        var createdEmployee = await response.Content.ReadFromJsonAsync<Employee>();

        var location = response!.Headers!.Location!.OriginalString;
        response = await client.DeleteAsync(location);
        response = await client.DeleteAsync(location);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
