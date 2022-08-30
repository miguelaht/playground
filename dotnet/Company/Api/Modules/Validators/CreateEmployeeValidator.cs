using Api.Modules.Contracts;
using Api.Modules.Ports;
using FluentValidation;

namespace Api.Modules.Validators
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
                .WithMessage("Email must be unique");
        }
    }
}

