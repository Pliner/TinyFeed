namespace TinyFeed.Core
{
    public interface IPackageBuilder
    {
        Package Build(byte[] bytes);
    }
}