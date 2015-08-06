using System.Linq;

namespace TinyFeed.Core
{
    public interface IPackageService
    {
        void Add(Package package);
        bool Any(string id, string version);
        Package FindLatestPackage(string id);
        Package FindPackage(string id, string version);
        IQueryable<Package> GetPackages();
        IQueryable<Package> FindPackagesById(string id);
    }
}