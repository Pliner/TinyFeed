using System.Linq;

namespace TinyFeed.Core
{
    public interface ITinyFeedPackageService
    {
        void Add(TinyFeedPackage package);
        TinyFeedPackage FindLatestPackage(string id);
        TinyFeedPackage FindPackage(string id, string version);
        IQueryable<TinyFeedPackage> GetPackages();
    }
}