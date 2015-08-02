using System;
using System.Data.Entity;

namespace TinyFeed.Core
{
    public interface ITinyFeedContext : IDisposable
    {
        IDbSet<TinyFeedPackage> Packages { get; }
        int SaveChanges();
        void Initialize();
    }
}