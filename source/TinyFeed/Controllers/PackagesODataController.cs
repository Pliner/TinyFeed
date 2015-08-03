using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using TinyFeed.Core;
using TinyFeed.Models;

namespace TinyFeed.Controllers
{
    public class PackagesODataController : ODataController
    {
        private readonly IPackageService packageService;

        public PackagesODataController(IPackageService packageService)
        {
            this.packageService = packageService;
        }

        [EnableQuery(PageSize = 20, HandleNullPropagation = HandleNullPropagationOption.False)]
        public IQueryable<ODataPackage> Get()
        {
            return packageService.GetPackages().Select(ToOData());
        }

        [HttpPost]
        [HttpGet]
        [EnableQuery(PageSize = 100, HandleNullPropagation = HandleNullPropagationOption.False)]
        public IHttpActionResult FindPackagesById([FromODataUri] string id)
        {
            return Ok(packageService.FindPackagesById(id).Select(ToOData()));
        }

        private static Expression<Func<Package, ODataPackage>> ToOData()
        {
            return x => new ODataPackage
            {
                Id = x.Id,
                Version = x.Version,
                Tags = x.Tags,
                Title = x.Title,
                IsLatestVersion = x.IsLatestVersion,
                LastUpdated = x.LastUpdated,
                Created = x.Created,
                Published = x.Published,
                PackageHash = x.PackageHash,
                PackageHashAlgorithm = x.PackageHashAlgorithm,
                Authors = x.Authors,
                Copyright = x.Copyright,
                Dependencies = x.Dependencies,
                Description = x.Description,
                DownloadCount = x.DownloadCount,
                IconUrl = x.IconUrl,
                LicenseUrl = x.LicenseUrl,
                PackageSize = x.PackageSize,
                ProjectUrl = x.ProjectUrl,
                ReleaseNotes = x.ReleaseNotes,
                RequireLicenseAcceptance = x.RequireLicenseAcceptance,
                Summary = x.Summary,
                VersionDownloadCount = x.VersionDownloadCount,
                IsAbsoluteLatestVersion = x.IsAbsoluteLatestVersion,
                IsPrerelease = x.IsPrerelease,
                DisplayTitle = x.DisplayTitle,
                DevelopmentDependency = x.DevelopmentDependency,
                Owners = x.Owners,
                Language = x.Language,
                Listed = x.Listed,
                Score = x.Score,
            };
        }
    }
}
