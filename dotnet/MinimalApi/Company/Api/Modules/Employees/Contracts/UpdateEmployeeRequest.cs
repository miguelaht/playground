using MediatR;

namespace Api.Modules.Employees.Contracts
{
    public class UpdateEmployeeRequest : IRequest<IResult>
    {
        public int Id { get; init; }
        public string Email { get; init; } = default!;
        public string FullName { get; init; } = default!;
        public DateTime BirthDate { get; init; }
    }
}
