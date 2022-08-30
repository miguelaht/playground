using Api.Modules.Employees.Contracts;
using Api.Modules.Employees.Ports;
using FluentValidation;
using MediatR;

namespace Api.Modules.Employees.Handlers
{
    public class CreateEmployee: IRequestHandler<CreateEmployeeRequest, IResult>
    {
        readonly IEmployeeRepository employees;
        readonly IValidator<CreateEmployeeRequest> validator;
        readonly LinkGenerator linker;

        public CreateEmployee(IEmployeeRepository employees, IValidator<CreateEmployeeRequest> validator, LinkGenerator linker)
        {
            this.employees = employees;
            this.validator = validator;
            this.linker = linker;
        }

        public async Task<IResult> Handle(CreateEmployeeRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var emp = await employees.Create(request.ToEmployee());

            if (emp is null)
            {
                return Results.Problem();
            }

            return Results.Created(linker.GetPathByName(nameof(ViewEmployee), values: new { id = emp.Id })!, emp);
        }
    }
}

