using System;

namespace TinyFeed.Core
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime UtcNow
        {
            get { return DateTime.UtcNow; }
        }
    }
}