using MediatR;

namespace Api.Modules.Employees.Contracts;

public class ViewEmployeeRequest : IRequest<IResult>
{
    public int id;
}
