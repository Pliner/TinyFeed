using System;

namespace TinyFeed.Core
{
    public interface IGuidGenerator
    {
        Guid NewGuid();
    }
}