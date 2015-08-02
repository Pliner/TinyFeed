using System.Linq;

namespace TinyFeed.Core
{
    public class TinyFeedPackageService : ITinyFeedPackageService
    {
        private readonly ITinyFeedContext context;
        
        public TinyFeedPackageService(ITinyFeedContext context)
        {
            this.context = context;
        }

        public void Add(TinyFeedPackage package)
        {
            context.Packages.Add(package);
            context.SaveChanges();
        }

        public TinyFeedPackage FindLatestPackage(string id)
        {
            return context.Packages.FirstOrDefault(x => x.Id == id && x.IsLatestVersion && x.IsAbsoluteLatestVersion);
        }

        public TinyFeedPackage FindPackage(string id, string version)
        {
            return context.Packages.FirstOrDefault(x => x.Id == id && x.Version == version);
        }

        public IQueryable<TinyFeedPackage> GetPackages()
        {
            return context.Packages;
        }
    }
}