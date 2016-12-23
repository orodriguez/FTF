using System;
using System.Data.Entity;
using DbContext = FTF.Core.DbContext;

namespace FTF.Specs
{
    public class Context
    {
        public DbContext Db { get; set; }

        public Func<DateTime> GetCurrentDate { get; set; }

        public DbContextTransaction Transaction { get; set; }
    }
}