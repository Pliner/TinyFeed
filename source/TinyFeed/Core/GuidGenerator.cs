using System;

namespace TinyFeed.Core
{
    public sealed class GuidGenerator : IGuidGenerator
    {
        public Guid NewGuid()
        {
            return Guid.NewGuid();
        }
    }
}