namespace TinyFeed
{
    public static class RouteNames
    {
        public const string Indexing = "Indexing";

        public static class Packages
        {
            public const string Search = "Packages.Search";
            public const string GetAvailableSearchFieldNames = "Packages.GetAvailableSearchFieldNames";
            public const string Upload = "Packages.Upload";
            public const string Delete = "Packages.Delete";
            public const string Info = "Packages.Info";
            public const string Download = "Packages.Download";
            public const string DownloadLatestVersion = "Packages.Download.Latest";
            public const string Feed = "OData Package Feed";
        }

        public static class Redirect
        {
            public const string RootFeed = "NuGetClient.Redirect.RootFeed";
            public const string ApiFeed = "NuGetClient.Redirect.ApiFeed";
            public const string Upload = "NuGetClient.Redirect.Upload";
            public const string Delete = "NuGetClient.Redirect.Delete";
        }

        public const string Sources = "Sources";

        public const string TabCompletionPackageIds = "Package Manager Console Tab Completion - Package IDs";
        public const string TabCompletionPackageVersions = "Package Manager Console Tab Completion - Package Versions";
    }
}
