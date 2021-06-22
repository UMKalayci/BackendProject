using System;
using System.Collections.Generic;
using System.Text;
using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class VolunteerValidator:AbstractValidator<Volunteer>
    {
        public VolunteerValidator()
        {
            RuleFor(p => p.UserId).GreaterThanOrEqualTo(1);
            RuleFor(p => p.CityId).GreaterThanOrEqualTo(1);
            RuleFor(p => p.Gender).InclusiveBetween(0, 1);
            RuleFor(p => p.Phone).NotNull();
        }

        private bool StartWithWithA(string arg)
        {
            return arg.StartsWith("A");
        }
    }
}
