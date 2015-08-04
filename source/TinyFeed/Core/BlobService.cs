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

        public void Upload(string blobPath, byte[] bytes)
        {
            var fullBlobPath = GetFullBlobPath(blobPath);
            EnsureDirectoryExists(fullBlobPath);
            if (File.Exists(fullBlobPath))
                return;
            using (var fileStream = File.Open(fullBlobPath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                fileStream.Write(bytes, 0, bytes.Length);
        }

        public Stream Download(string blobPath)
        {
            return File.Open(GetFullBlobPath(blobPath), FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        public bool HasBlob(string blobPath)
        {
            return File.Exists(blobPath);
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

        private string GetFullBlobPath(string blobPath)
        {
            return Path.Combine(baseBlobPath, blobPath);
        }
    }
}