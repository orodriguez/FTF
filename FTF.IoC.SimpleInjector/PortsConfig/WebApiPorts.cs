using System;
using System.Data.Entity;
using FTF.Core.Delegates;
using FTF.Core.Entities;
using FTF.Core.Ports;
using DbContext = FTF.Core.EntityFramework.DbContext;

namespace FTF.IoC.SimpleInjector.PortsConfig
{
    public class WebApiPorts : IPorts
    {
        public GetCurrentTime GetCurrentTime { get; }
        public IAuth Auth { get; }
        public IStorage Storage { get; }

        public WebApiPorts()
        {
            GetCurrentTime = () => DateTime.Now;
            Auth = new Auth();
            Storage = new Storage();
        }
    }

    public class Auth : IAuth
    {
        public User CurrentUser
        {
            get { return new User { Name = "orodriguez"}; }
            set { throw new NotImplementedException();}
        }
    }

    public class Storage : IStorage
    {
        public void Dispose()
        {
        }

        public DbContext MakeDbContext() => 
            new DbContext("name=FTF.UAT", new CreateDatabaseIfNotExists<DbContext>());
    }
}