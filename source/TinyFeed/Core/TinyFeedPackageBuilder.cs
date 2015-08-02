using System.IO;
using NuGet;

namespace TinyFeed.Core
{
    public class TinyFeedPackageBuilder : ITinyFeedPackageBuilder
    {
        private readonly ICryptoService cryptoService;
        private readonly IDateTimeService dateTimeService;

        public TinyFeedPackageBuilder(ICryptoService cryptoService, IDateTimeService dateTimeService)
        {
            this.cryptoService = cryptoService;
            this.dateTimeService = dateTimeService;
        }

        public TinyFeedPackage Build(byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes))
            {
                var package = new ZipPackage(stream);
                var now = dateTimeService.UtcNow;
                return new TinyFeedPackage
                {
                    Id = package.Id,
                    Version = package.Version.ToString(),
                    DisplayTitle = package.Title.ToStringOrEmpty(),
                    IsAbsoluteLatestVersion = package.IsAbsoluteLatestVersion,
                    IsLatestVersion = package.IsLatestVersion,
                    IsPrerelease = package.IsPrerelease(),
                    PackageHash = cryptoService.Hash(bytes),
                    PackageHashAlgorithm = cryptoService.HashAlgorithmId,
                    PackageSize = bytes.LongLength,
                    Created = now,
                    LastUpdated = now,
                    Published = now,
                    Owners = package.Owners.Flatten(),
                    Authors = package.Authors.Flatten(),
                    Listed = true,
                    RequireLicenseAcceptance = package.RequireLicenseAcceptance,
                    Language = package.Language.ToStringOrEmpty(),
                    DevelopmentDependency = package.DevelopmentDependency,
                    Title = package.Title.ToStringOrEmpty(),
                    Tags = package.Tags.ToStringOrEmpty(),
                    Copyright = package.Copyright.ToStringOrEmpty(),
                    Dependencies = "",
                    IconUrl = package.IconUrl.ToStringOrEmpty(),
                    LicenseUrl = package.LicenseUrl.ToStringOrEmpty(),
                    ProjectUrl = package.ProjectUrl.ToStringOrEmpty(),
                    Description = package.Description.ToStringOrEmpty(),
                    ReleaseNotes = package.ReleaseNotes.ToStringOrEmpty(),
                    Summary = package.Summary.ToStringOrEmpty(),
                    DownloadCount = 0,
                    Score = 0f,
                    VersionDownloadCount = 0
                };
            }
        }
    }
}