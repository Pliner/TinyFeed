using System;
using System.Security.Cryptography;

namespace TinyFeed.Core
{
    public sealed class CryptoService : ICryptoService
    {
        public string HashAlgorithmId
        {
            get { return "SHA512"; }
        }

        public string Hash(byte[] bytes)
        {
            using (var hashAlgorithm = HashAlgorithm.Create("SHA512"))
            {
                var hashBytes = hashAlgorithm.ComputeHash(bytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}