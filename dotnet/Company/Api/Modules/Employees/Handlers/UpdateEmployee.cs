using Api.Modules.Employees.Contracts;
using Api.Modules.Employees.Ports;
using FluentValidation;
using MediatR;

namespace Api.Modules.Employees.Handlers
{
    public class UpdateEmployee : IRequestHandler<UpdateEmployeeRequest, IResult>
    {
        private readonly IEmployeeRepository employees;
        private readonly IValidator<UpdateEmployeeRequest> validator;

        public UpdateEmployee(IEmployeeRepository employees, IValidator<UpdateEmployeeRequest> validator)
        {
            this.employees = employees;
            this.validator = validator;
        }

        public async Task<IResult> Handle(UpdateEmployeeRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var emp = await employees.View(request.Id);

            if (emp is null)
            {
                return Results.NotFound();
            }

            var updatedEmp = await employees.Update(request.ToEmployee());

            if (updatedEmp is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(updatedEmp);
        }
    }
}

