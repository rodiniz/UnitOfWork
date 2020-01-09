using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Inyector.Attributes;

namespace UnitOfWork.Host.Validators
{
    [Inyect(typeof(IValidator<BlogModel>))]
    public class BlogValidator : AbstractValidator<BlogModel>
    {
        public BlogValidator()
        {
            RuleFor(c => c.Title).NotEmpty();
        }
    }
}
