namespace TinyFeed.Core
{
    public interface ICryptoService
    {
        string HashAlgorithmId { get; }
        string Hash(byte[] bytes);
    }
}