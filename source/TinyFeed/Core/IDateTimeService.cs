using System;

namespace TinyFeed.Core
{
    public interface IDateTimeService
    {
        DateTime UtcNow { get; }
    }
}