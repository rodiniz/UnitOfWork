using System.Collections.Generic;

namespace Arch.EntityFrameworkCore.UnitOfWork.Tests.Entities
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int CountryId { get; set; }

        public virtual Country Country { get; set; }

        public List<Town> Towns { get; set; }
    }
}