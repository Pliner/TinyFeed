using System.IO;

namespace TinyFeed.Core
{
    public class TinyFeedBlobService : ITinyFeedBlobService
    {
        private readonly string baseBlobPath;

        public TinyFeedBlobService(string baseBlobPath)
        {
            this.baseBlobPath = baseBlobPath;
        }

        public void Upload(string blobPath, byte[] bytes)
        {
            var fullBlobPath = GetFullBlobPath(blobPath);
            EnsureDirectoryExists(fullBlobPath);
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

        private string GetFullBlobPath(string blobPath)
        {
            return Path.Combine(baseBlobPath, blobPath);
        }

    }
}