using Api.Modules.Employees.Contracts;
using Api.Modules.Employees.Ports;
using FluentValidation;

namespace Api.Modules.Employees.Validators
{
    public class UpdateEmployeeValidator : AbstractValidator<UpdateEmployeeRequest>
    {
        private readonly IEmployeeRepository _employees;

        public UpdateEmployeeValidator(IEmployeeRepository employees)
        {
            _employees = employees;
            RuleFor(e => e.Id).NotEmpty();
            RuleFor(e => e.FullName).NotEmpty();
            RuleFor(e => e.BirthDate.Year).LessThan(DateTime.Now.Year);
            RuleFor(e => e.Email)
                .NotEmpty()
                .EmailAddress()
                .MustAsync(
                    async (emp, email, _) =>
                    {
                        var exists = await _employees.ViewByEmail(email);

                        if (exists is null || exists.Id == emp.Id)
                            return true;

                        return false;
                    }
                )
                .WithMessage("Email must be unique");
        }
    }
}
