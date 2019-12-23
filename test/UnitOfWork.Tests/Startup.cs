using System;
using System.Collections.Generic;
using System.Text;
using Arch.EntityFrameworkCore.UnitOfWork;
using Arch.EntityFrameworkCore.UnitOfWork.Tests;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;
using Xunit.DependencyInjection;


[assembly: TestFramework("UnitOfWork.Tests.Startup", "UnitOfWork.Tests")]
namespace UnitOfWork.Tests
{
    public class Startup : DependencyInjectionTestFramework
    {
        public Startup(IMessageSink messageSink) : base(messageSink) { }

        protected override void ConfigureServices(IServiceCollection services)
        {
            services
               .AddDbContext<InMemoryContext>()
               //.AddDbContext<BloggingContext>(opt => opt.UseInMemoryDatabase("UnitOfWork"))
               .AddUnitOfWork<InMemoryContext>();              
        }
    }
}
