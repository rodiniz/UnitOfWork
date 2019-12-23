using Arch.EntityFrameworkCore.UnitOfWork;
using Arch.EntityFrameworkCore.UnitOfWork.Tests.Entities;
using Xunit;
using System.Linq;
using AutoFixture;
using System.Threading.Tasks;

namespace UnitOfWork.Tests
{
    public class TestInjection
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<City> repository;
        public Fixture _fixture => new Fixture();
        public TestInjection(IUnitOfWork unitOfWork)
        {         
            _unitOfWork = unitOfWork;
            repository = _unitOfWork.GetRepository<City>();
            if (repository.Count() == 0)
            {
                Task.WaitAll(InitAsync());
            }
            
        }

        private async Task InitAsync()
        {
            var cities = _fixture.CreateMany<City>();           
            await repository.InsertAsync(cities);
            await _unitOfWork.SaveChangesAsync();
        }
        [Fact]
        public void GetPagedList()
        { 
            var page = repository.GetPagedList(pageSize: 1);
            Assert.Equal(1, page.Items.Count);                     
        }
    }
}
