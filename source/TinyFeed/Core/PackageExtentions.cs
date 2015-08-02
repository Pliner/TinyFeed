using System.Collections.Generic;
using NuGet;

namespace TinyFeed.Core
{
    public static class PackageExtentions
    {
        public static bool IsPrerelease(this IPackage packageMetadata)
        {
            return string.IsNullOrEmpty(packageMetadata.Version.SpecialVersion);
        }

        public static string Flatten(this IEnumerable<string> elements)
        {
            return string.Join(", ", elements);
        }

        public static string ToStringOrEmpty<T>(this T value)
        {
            return value == null ? "" : value.ToString();
        }
    }
}