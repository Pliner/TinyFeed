using System.Data.Entity;

namespace TinyFeed.Core
{
    public interface ITinyFeedContext
    {
        IDbSet<TinyFeedPackage> Packages { get; }
        int SaveChanges();
        void Initialize();
    }
}