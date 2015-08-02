using System;
using System.Data.Entity;

namespace TinyFeed.Core
{
    public interface IContext : IDisposable
    {
        IDbSet<Package> Packages { get; }
        int SaveChanges();
        void Initialize();
    }
}