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