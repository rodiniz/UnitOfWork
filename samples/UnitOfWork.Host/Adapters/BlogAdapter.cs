using Arch.EntityFrameworkCore.UnitOfWork.Host.Models;
using Inyector.Attributes;

namespace UnitOfWork.Host.Adapters
{
    [Inyect(typeof(IAdapter<Blog, BlogModel>))]
    public class BlogAdapter : IAdapter<Blog, BlogModel>
    {
        public Blog convertFromModel(BlogModel model)
        {
            return new Blog { Id = model.Id, Title = model.Title, Url = model.Url };
        }

        public BlogModel convertToModel(Blog entity)
        {
            return new BlogModel { Id = entity.Id, Url = entity.Url, Title = entity.Title };
        }
    }
}