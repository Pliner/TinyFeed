namespace TinyFeed.Core
{
    public interface ITinyFeedPackageBuilder
    {
        TinyFeedPackage Build(byte[] bytes);
    }
}