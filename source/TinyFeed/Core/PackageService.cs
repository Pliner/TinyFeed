using System.Linq;
using NuGet;

namespace TinyFeed.Core
{
    public class PackageService : IPackageService
    {
        private readonly IContext context;
        
        public PackageService(IContext context)
        {
            this.context = context;
        }

        public void Add(Package package)
        {
            foreach (var p in context.Packages.Where(x => x.Id == package.Id))
            {
                p.IsLatestVersion = false;
                p.IsAbsoluteLatestVersion = false;
            }

            var latestPackage = context.Packages.Where(x => x.Id == package.Id)
                .ToArray()
                .DefaultIfEmpty(package)
                .OrderBy(x => new SemanticVersion(x.Version))
                .First();
            
            latestPackage.IsLatestVersion = true;
            latestPackage.IsAbsoluteLatestVersion = true;

            context.Packages.Add(package); 
            context.SaveChanges();
        }

        public Package FindLatestPackage(string id)
        {
            return context.Packages.FirstOrDefault(x => x.Id == id && x.IsLatestVersion && x.IsAbsoluteLatestVersion);
        }

        public Package FindPackage(string id, string version)
        {
            return context.Packages.FirstOrDefault(x => x.Id == id && x.Version == version);
        }

        public IQueryable<Package> GetPackages()
        {
            return context.Packages;
        }
    }
}