using System;

namespace TinyFeed.Core
{
    public sealed class DateTimeService : IDateTimeService
    {
        public DateTime UtcNow
        {
            get { return DateTime.UtcNow; }
        }
    }
}