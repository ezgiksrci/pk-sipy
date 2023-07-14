using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SipayApi.Controllers
{
    public class Person
    {
        [DisplayName("Staff person name")]
        public string Name { get; set; }

        [DisplayName("Staff person lastname")]
        public string Lastname { get; set; }

        [DisplayName("Staff person phone number")]
        public string Phone { get; set; }

        [DisplayName("Staff person access level to system")]
        public int AccessLevel { get; set; }

        [DisplayName("Staff person salary")]
        public decimal Salary { get; set; }
    }

    public class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Staff person name is required.")
                .Length(5, 100).WithMessage("Staff person name must be between 5 and 100 characters.");

            RuleFor(x => x.Lastname)
                .NotEmpty().WithMessage("Staff person lastname is required.")
                .Length(5, 100).WithMessage("Staff person lastname must be between 5 and 100 characters.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Staff person phone number is required.")
                .Matches(@"^\d{10}$").WithMessage("Staff person phone number must be a valid 10-digit phone number.");

            RuleFor(x => x.AccessLevel)
                .InclusiveBetween(1, 5).WithMessage("Staff person access level to system must be between 1 and 5.");

            RuleFor(x => x.Salary)
                .Must((person, salary) => IsValidSalary(person.AccessLevel, salary))
                .WithMessage("Invalid salary for the specified access level.");
        }

        private bool IsValidSalary(int accessLevel, decimal salary)
        {
            switch (accessLevel)
            {
                case 1:
                    return salary <= 10000;
                case 2:
                    return salary <= 20000;
                case 3:
                    return salary <= 30000;
                case 4:
                    return salary <= 40000;
                default:
                    return false;
            }
        }
    }

    [ApiController]
    [Route("sipay/api/[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IValidator<Person> _validator;

        public PersonController(IValidator<Person> validator)
        {
            _validator = validator;
        }

        [HttpPost]
        public IActionResult Post(Person person)
        {
            var validationResult = _validator.Validate(person);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            return Ok(person);
        }
    }
}