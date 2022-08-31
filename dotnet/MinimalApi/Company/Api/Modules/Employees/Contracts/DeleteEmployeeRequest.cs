using MediatR;

namespace Api.Modules.Employees.Contracts;

public class DeleteEmployeeRequest : IRequest<IResult>
{
    public int Id { get; init; }
}

