using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace UnitOfWork.Host.Validators
{
    public class BlogValidator : AbstractValidator<BlogModel>
    {
        public BlogValidator()
        {
            RuleFor(c => c.Title).NotEmpty();
        }
    }
}
