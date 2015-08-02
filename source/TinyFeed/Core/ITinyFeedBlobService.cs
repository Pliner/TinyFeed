using System.IO;

namespace TinyFeed.Core
{
    public interface ITinyFeedBlobService
    {
        void Upload(string blobPath, byte[] bytes);
        Stream Download(string blobPath);
    }
}