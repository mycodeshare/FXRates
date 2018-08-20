using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TestSite1.Models
{
    public class TestSite1Context : DbContext
    {
        public TestSite1Context (DbContextOptions<TestSite1Context> options)
            : base(options)
        {
        }

        public DbSet<TestSite1.Models.BaseCurrency> BaseCurrency { get; set; }
        public DbSet<TestSite1.Models.Rates> Rates { get; set; }
    }
}
