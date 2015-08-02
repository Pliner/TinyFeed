using System.Linq;

namespace TinyFeed.Core
{
    public interface IPackageService
    {
        void Add(Package package);
        Package FindLatestPackage(string id);
        Package FindPackage(string id, string version);
        IQueryable<Package> GetPackages();
    }
}