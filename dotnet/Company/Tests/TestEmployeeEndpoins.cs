using Api.Modules.Contracts;
using Api.Modules.Core;
using Bogus;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Net;

namespace Tests;

public class TestEmployeeEndpoints : IClassFixture<WebApplicationFactory<Api.Program>>
{
    private readonly HttpClient client;

    public TestEmployeeEndpoints(WebApplicationFactory<Api.Program> app)
    {
        client = app.CreateClient();
    }

    private Faker<CreateEmployeeRequest> createEmployeeFaker()
    {
        var f = new Faker<CreateEmployeeRequest>();

        f.RuleFor(e => e.FullName, f => f.Person.FullName);
        f.RuleFor(e => e.Email, f => f.Person.Email);
        f.RuleFor(e => e.BirthDate, f => f.Person.DateOfBirth);

        return f;
    }

    [Fact]
    public async Task TestCreateEmployee_ValidData()
    {
        var generator = createEmployeeFaker();
        var newUser = generator.Generate();

        var response = await client.PostAsJsonAsync("/employees", newUser);
        var createdUser = await response.Content.ReadFromJsonAsync<Employee>();

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(createdUser);
        Assert.Equal(newUser.FullName, createdUser.FullName);
        Assert.Equal(newUser.Email, createdUser.Email);
        Assert.Equal(newUser.BirthDate.ToString(), createdUser.BirthDate.ToString());
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
        var generator = createEmployeeFaker();
        var newUser = generator.Generate();

        var response = await client.PostAsJsonAsync("/employees", newUser);
        var createdUser = await response.Content.ReadFromJsonAsync<Employee>();

        var location = response!.Headers!.Location!.OriginalString;
        response = await client.GetAsync(location);
        var viewEmployee = await response.Content.ReadFromJsonAsync<Employee>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(createdUser, viewEmployee);
    }

    [Fact]
    public async Task TestViewEmployee_NotValidId()
    {
        var generator = createEmployeeFaker();
        var newUser = generator.Generate();

        var response = await client.PostAsJsonAsync("/employees", newUser);
        var createdUser = await response.Content.ReadFromJsonAsync<Employee>();

        var location = response!.Headers!.Location!.OriginalString;
        response = await client.DeleteAsync(location);
        response = await client.GetAsync(location);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task TestDeleteEmployee_ValidId()
    {
        var generator = createEmployeeFaker();
        var newUser = generator.Generate();

        var response = await client.PostAsJsonAsync("/employees", newUser);
        var createdUser = await response.Content.ReadFromJsonAsync<Employee>();

        var location = response!.Headers!.Location!.OriginalString;
        response = await client.DeleteAsync(location);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task TestDeleteEmployee_NotValidId()
    {
        var generator = createEmployeeFaker();
        var newUser = generator.Generate();

        var response = await client.PostAsJsonAsync("/employees", newUser);
        var createdUser = await response.Content.ReadFromJsonAsync<Employee>();

        var location = response!.Headers!.Location!.OriginalString;
        response = await client.DeleteAsync(location);
        response = await client.DeleteAsync(location);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
