using Arch.EntityFrameworkCore.UnitOfWork;
using Arch.EntityFrameworkCore.UnitOfWork.Host.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace UnitOfWork.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : BaseCrudController<Blog, BlogModel>
    {
        public BlogController(IUnitOfWork unitOfWork, IAdapter<Blog, BlogModel> adapter, IValidator<BlogModel> validator) : base(unitOfWork, adapter, validator)
        {
        }
    }
}