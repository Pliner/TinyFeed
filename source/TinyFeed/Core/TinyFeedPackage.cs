using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TinyFeed.Core
{
    [Table("TinyFeedPackages")]
    public class TinyFeedPackage
    {
        [Column(Order = 0), Key]
        public string Id { get; set; }

        [Column(Order = 1), Key]
        public string Version { get; set; }

        public string Title { get; set; }

        public string DisplayTitle { get; set; }

        public string Authors { get; set; }

        public string Owners { get; set; }

        public string IconUrl { get; set; }

        public string LicenseUrl { get; set; }

        public string ProjectUrl { get; set; }

        public int DownloadCount { get; set; }

        public bool RequireLicenseAcceptance { get; set; }

        public string Description { get; set; }

        public string Summary { get; set; }

        public string ReleaseNotes { get; set; }

        public string Language { get; set; }

        public DateTime Created { get; set; }

        public DateTime Published { get; set; }

        public DateTime LastUpdated { get; set; }

        public string Dependencies { get; set; }

        public string PackageHash { get; set; }

        public string PackageHashAlgorithm { get; set; }

        public long PackageSize { get; set; }

        public string Copyright { get; set; }

        public string Tags { get; set; }

        public bool IsAbsoluteLatestVersion { get; set; }

        public bool IsLatestVersion { get; set; }

        public bool IsPrerelease { get; set; }

        public bool Listed { get; set; }

        public bool DevelopmentDependency { get; set; }

        public int VersionDownloadCount { get; set; }

        public float Score { get; set; }
    }
}