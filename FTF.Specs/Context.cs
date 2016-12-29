using System;
using System.Data.Entity;
using FTF.Core.Entities;
using DbContext = FTF.Storage.EntityFramework.DbContext;

namespace FTF.Specs
{
    public class Context
    {
        public User CurrentUser { get; set; }

        public DbContext Db { get; set; }

        public Func<DateTime> GetCurrentDate { get; set; }

        public DbContextTransaction Transaction { get; set; }

        public Context()
        {
            GetCurrentDate = () => DateTime.Now;
        }
    }
}