namespace TinyFeed
{
    public static class RouteNames
    {
        public static class Packages
        {
            public const string GetAvailableSearchFieldNames = "Packages.GetAvailableSearchFieldNames";
            public const string Upload = "Packages.Upload";
            public const string Download = "Packages.Download";
            public const string DownloadLatestVersion = "Packages.Download.Latest";
            public const string Feed = "OData Package Feed";
        }

        public static class Redirect
        {
            public const string RootFeed = "NuGetClient.Redirect.RootFeed";
            public const string ApiFeed = "NuGetClient.Redirect.ApiFeed";
            public const string Upload = "NuGetClient.Redirect.Upload";
        }
    }
}
