﻿using System.Linq;
using System.Threading.Tasks;
using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using Arch.EntityFrameworkCore.UnitOfWork.Host.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Arch.EntityFrameworkCore.UnitOfWork.Host.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        // 1. IRepositoryFactory used for readonly scenario;
        // 2. IUnitOfWork used for read/write scenario;
        // 3. IUnitOfWork<TContext> used for multiple databases scenario;
        public ValuesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET api/values
        [HttpGet]
        public async Task<IPagedList<Blog>> Get()
        {
            return await _unitOfWork.GetRepository<Blog>().GetPagedListAsync(include: source => source.Include(blog => blog.Posts).ThenInclude(post => post.Comments));
        }

        // GET api/values/Page/5/10
        [HttpGet("Page/{pageIndex}/{pageSize}")]
        public async Task<IPagedList<Blog>> Get(int pageIndex, int pageSize)
        {
            // projection
            var items = _unitOfWork.GetRepository<Blog>().GetPagedList(b => new { Name = b.Title, Link = b.Url });

            return await _unitOfWork.GetRepository<Blog>().GetPagedListAsync(pageIndex: pageIndex, pageSize: pageSize);
        }

        // GET api/values/Search/a1
        [HttpGet("Search/{term}")]
        public async Task<IPagedList<Blog>> Get(string term)
        {
            var item = _unitOfWork.GetRepository<Blog>().GetFirstOrDefault(predicate: x => x.Title.Contains(term), include: source => source.Include(blog => blog.Posts).ThenInclude(post => post.Comments));

            item = _unitOfWork.GetRepository<Blog>().GetFirstOrDefault(predicate: x => x.Title.Contains(term), orderBy: source => source.OrderByDescending(b => b.Id));

            var projection = _unitOfWork.GetRepository<Blog>().GetFirstOrDefault(b => new { Name = b.Title, Link = b.Url }, predicate: x => x.Title.Contains(term));

            return await _unitOfWork.GetRepository<Blog>().GetPagedListAsync(predicate: x => x.Title.Contains(term));
        }

        // GET api/values/4
        [HttpGet("{id}")]
        public async Task<Blog> Get(int id)
        {
            return await _unitOfWork.GetRepository<Blog>().FindAsync(id);
        }

        // POST api/values
        [HttpPost]
        public async Task Post([FromBody]Blog value)
        {
            var repo = _unitOfWork.GetRepository<Blog>();
            await repo.InsertAsync(value);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}