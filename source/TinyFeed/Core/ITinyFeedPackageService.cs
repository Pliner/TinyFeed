using System.IO;
using System.Linq;

namespace TinyFeed.Core
{
    public interface ITinyFeedPackageService
    {
        void AddPackage(Stream stream);
        TinyFeedPackage FindLatestPackage(string id);
        TinyFeedPackage FindPackage(string id, string version);
        IQueryable<TinyFeedPackage> GetPackages();
        Stream GetStream(string id, string version);
    }
}