using System.IO;

namespace TinyFeed.Core
{
    public class BlobService : IBlobService
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
            using (var fileStream = File.OpenWrite(fullBlobPath))
                fileStream.Write(bytes, 0, bytes.Length);
        }

        private static void EnsureDirectoryExists(string fullBlobPath)
        {
            var directoryInfo = Directory.GetParent(fullBlobPath);
            Directory.CreateDirectory(directoryInfo.FullName);
        }

        public Stream Download(string blobPath)
        {
            return File.OpenRead(GetFullBlobPath(blobPath));
        }

        public bool HasBlob(string blobPath)
        {
            return File.Exists(blobPath);
        }

        private string GetFullBlobPath(string blobPath)
        {
            return Path.Combine(baseBlobPath, blobPath);
        }
    }
}