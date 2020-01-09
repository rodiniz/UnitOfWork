using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using Arch.EntityFrameworkCore.UnitOfWork.Tests.Entities;
using AutoFixture;
using Xunit;

namespace Arch.EntityFrameworkCore.UnitOfWork.Tests
{
    public class IQueryablePageListExtensionsTests
    {
        public Fixture _fixture => new Fixture();

        [Fact]
        public async Task ToPagedListAsyncTest()
        {
            using (var db = new InMemoryContext())
            {
                var testItems = TestItems();
                await db.AddRangeAsync(testItems);
                db.SaveChanges();

                var items = db.Customers.Where(t => t.Age > 1);

                var page = await items.ToPagedListAsync(1, 2);
                Assert.NotNull(page);

                Assert.Equal(4, page.TotalCount);
                Assert.Equal(2, page.Items.Count);

                page = await items.ToPagedListAsync(0, 2);
                Assert.NotNull(page);
                Assert.Equal(4, page.TotalCount);
                Assert.Equal(2, page.Items.Count);
            }
        }

        public List<Customer> TestItems()
        {
            return _fixture.CreateMany<Customer>(4).ToList();
        }
    }
}