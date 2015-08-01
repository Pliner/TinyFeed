namespace TinyFeed
{
    public interface INuGetWebApiSettings
    {
        // Security
        bool ShowExceptionDetails { get; }

        bool EnableCrossDomainRequests { get; }

        // Web
        string RoutePathPrefix { get; }
    }
}
