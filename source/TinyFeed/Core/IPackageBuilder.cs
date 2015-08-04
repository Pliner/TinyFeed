namespace TinyFeed.Core
{
    public interface IPackageBuilder
    {
        bool TryBuild(byte[] bytes, out Package package);
    }
}