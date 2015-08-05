using System;
using System.IO;

namespace TinyFeed.Core
{
    public sealed class BlobService : IBlobService
    {
        private readonly string baseBlobPath;

        public BlobService(string baseBlobPath)
        {
            this.baseBlobPath = baseBlobPath;
        }

        public void Upload(string scope, Guid id, byte[] bytes)
        {
            var fullBlobPath = GetFullBlobPath(scope, id);
            EnsureDirectoryExists(fullBlobPath);
            if (File.Exists(fullBlobPath))
            {
                return;
            }
            using (var fileStream = File.Open(fullBlobPath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                fileStream.Write(bytes, 0, bytes.Length);
            }
        }

        public Stream Download(string scope, Guid id)
        {
            return File.Open(GetFullBlobPath(scope, id), FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        public bool HasBlob(string scope, Guid id)
        {
            return File.Exists(GetFullBlobPath(scope, id));
        }

        private static void EnsureDirectoryExists(string fullBlobPath)
        {
            try
            {
                var directoryInfo = Directory.GetParent(fullBlobPath);
                Directory.CreateDirectory(directoryInfo.FullName);
            }
            catch (Exception)
            {
            }
        }

        private string GetFullBlobPath(string scope, Guid id)
        {
            return Path.Combine(Path.Combine(baseBlobPath, scope), id.ToString());
        }
    }
}