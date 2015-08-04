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

        public static string ToStringSafe<T>(this T value)
        {
            if (value == null)
                return "";
            var stringValue = value.ToString();
            if (stringValue.Length <= Constraints.MaxStringLength)
                return stringValue;
            return stringValue.Substring(0, Constraints.MaxStringLength);
        }

        public static bool IsTooLargeString(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;
            return value.Length > Constraints.MaxStringLength;
        }
    }
}