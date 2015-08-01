using System;
using System.IO;
using System.Linq;

namespace TinyFeed.Core
{
    public class TinyFeedPackageService : ITinyFeedPackageService
    {
        public void AddPackage(Stream stream)
        {
            throw new NotImplementedException();
        }

        public TinyFeedPackage FindLatestPackage(string id)
        {
            throw new NotImplementedException();
        }

        public TinyFeedPackage FindPackage(string id, string version)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TinyFeedPackage> GetPackages()
        {
            throw new NotImplementedException();
        }

        public Stream GetStream(string id, string version)
        {
            throw new NotImplementedException();
        }
    }
}