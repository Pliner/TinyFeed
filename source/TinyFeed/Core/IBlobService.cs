using System;
using System.IO;

namespace TinyFeed.Core
{
    public interface IBlobService
    {
        void Upload(string scope, Guid id, byte[] bytes);
        Stream Download(string scope, Guid id);
        bool HasBlob(string scope, Guid id);
    }
}