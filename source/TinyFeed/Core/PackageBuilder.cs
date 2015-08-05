using System;
using System.IO;
using NuGet;

namespace TinyFeed.Core
{
    public sealed class PackageBuilder : IPackageBuilder
    {
        private readonly ICryptoService cryptoService;
        private readonly IDateTimeService dateTimeService;
        private readonly IGuidGenerator guidGenerator;

        public PackageBuilder(ICryptoService cryptoService, 
                              IDateTimeService dateTimeService,
                              IGuidGenerator guidGenerator
            )
        {
            this.cryptoService = cryptoService;
            this.dateTimeService = dateTimeService;
            this.guidGenerator = guidGenerator;
        }

        public bool TryBuild(byte[] bytes, out Package package)
        {
            package = null;
            try
            {
                using (var stream = new MemoryStream(bytes))
                {
                    var zipPackage = new ZipPackage(stream);
                    if (zipPackage.Id.IsTooLargeString() || zipPackage.Version.ToString().IsTooLargeString())
                    {
                        return false;
                    }

                    var now = dateTimeService.UtcNow;
                    package = new Package
                    {
                        Id = zipPackage.Id,
                        Version = zipPackage.Version.ToString(),
                        DisplayTitle = zipPackage.Title.ToStringSafe(),
                        IsAbsoluteLatestVersion = zipPackage.IsAbsoluteLatestVersion,
                        IsLatestVersion = zipPackage.IsLatestVersion,
                        IsPrerelease = zipPackage.IsPrerelease(),
                        PackageHash = cryptoService.Hash(bytes),
                        PackageHashAlgorithm = cryptoService.HashAlgorithmId,
                        PackageSize = bytes.LongLength,
                        Created = now,
                        LastUpdated = now,
                        Published = now,
                        Owners = zipPackage.Owners.Flatten().ToStringSafe(),
                        Authors = zipPackage.Authors.Flatten().ToStringSafe(),
                        Listed = true,
                        RequireLicenseAcceptance = zipPackage.RequireLicenseAcceptance,
                        Language = zipPackage.Language.ToStringSafe(),
                        DevelopmentDependency = zipPackage.DevelopmentDependency,
                        Title = zipPackage.Title.ToStringSafe(),
                        Tags = zipPackage.Tags.ToStringSafe(),
                        Copyright = zipPackage.Copyright.ToStringSafe(),
                        Dependencies = "".ToStringSafe(),
                        IconUrl = zipPackage.IconUrl.ToStringSafe(),
                        LicenseUrl = zipPackage.LicenseUrl.ToStringSafe(),
                        ProjectUrl = zipPackage.ProjectUrl.ToStringSafe(),
                        Description = zipPackage.Description.ToStringSafe(),
                        ReleaseNotes = zipPackage.ReleaseNotes.ToStringSafe(),
                        Summary = zipPackage.Summary.ToStringSafe(),
                        DownloadCount = 0,
                        Score = 0f,
                        VersionDownloadCount = 0,
                        BlobId = guidGenerator.NewGuid()
                    };
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}