using System.IO;

namespace TinyFeed.Core
{
    public interface IBlobService
    {
        void Upload(string blobPath, byte[] bytes);
        Stream Download(string blobPath);
        bool HasBlob(string blobPath);
    }
}