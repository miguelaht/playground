using Api.Modules.Employees.Contracts;
using Api.Modules.Employees.Ports;
using FluentValidation;

namespace Api.Modules.Employees.Validators
{
    public class CreateEmployeeValidator : AbstractValidator<CreateEmployeeRequest>
    {
        private readonly IEmployeeRepository _employees;
        public CreateEmployeeValidator(IEmployeeRepository employees)
        {
            _employees = employees;

            RuleFor(e => e.FullName).NotEmpty();
            RuleFor(e => e.BirthDate.Year).LessThan(DateTime.Now.Year);
            RuleFor(e => e.Email)
                .NotEmpty()
                .EmailAddress()
                .MustAsync(async (_, email, _) =>
                {
                    var emp = await _employees.ViewByEmail(email);

                    return emp is null;
                })
                .WithMessage($"{nameof(CreateEmployeeRequest.Email)} must be unique");
        }
    }
}

