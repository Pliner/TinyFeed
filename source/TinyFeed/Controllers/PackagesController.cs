using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Web.Http;
using NuGet;
using TinyFeed.Core;
using IPackageBuilder = TinyFeed.Core.IPackageBuilder;

namespace TinyFeed.Controllers
{
    /// <summary>
    /// Provides methods to search, get metadata, download, upload and delete packages.
    /// </summary>
    public class PackagesController : ApiController
    {
        private readonly IPackageBuilder packageBuilder;
        private readonly IPackageService packageService;
        private readonly IBlobService blobService;

        public PackagesController(IPackageBuilder packageBuilder, 
                                  IPackageService packageService, 
                                  IBlobService blobService)
        {
            this.packageBuilder = packageBuilder;
            this.packageService = packageService;
            this.blobService = blobService;
        }

        /// <summary>
        /// Downloads the complete <c>.nupkg</c> content. The HTTP HEAD method
        /// is also supported for verifying package size, and modification date.
        /// The <c>ETag</c> response header will contain the md5 hash of the
        /// package content.
        /// </summary>
        [HttpGet, HttpHead]
        public HttpResponseMessage DownloadPackage(string id, string version = null)
        {
            var package = version == null
                ? packageService.FindLatestPackage(id)
                : packageService.FindPackage(id, version);

            var response = Request.CreateResponse(HttpStatusCode.OK);

            if (Request.Method == HttpMethod.Get)
            {
                response.Content = new StreamContent(blobService.Download(GetFilepath(package)));
            }
            else
            {
                response.Content = new StringContent(string.Empty);
            }

            response.Headers.ETag = new EntityTagHeaderValue('"' + package.PackageHash + '"');
            response.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/zip");
            response.Content.Headers.LastModified = package.LastUpdated;
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue(DispositionTypeNames.Attachment)
            {
                FileName = GetFilepath(package),
                CreationDate = package.LastUpdated,                                                                 
                ModificationDate = package.LastUpdated
            };

            return response;
        }

        private static string GetFilepath(Package package)
        {
            return Path.Combine(package.Id, string.Format("{0}.{1}{2}", package.Id, package.Version, Constants.PackageExtension));
        }

        /// <summary>
        /// Upload a package to the repository. If a package already exists
        /// with the same Id and Version, it will be replaced with the new package.
        /// </summary>
        [HttpPut]
        [HttpPost]
        public HttpResponseMessage PutPackage(HttpRequestMessage message)
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            var provider = new MultipartFormDataStreamProvider(Path.GetTempPath());

            try
            {
                message.Content.ReadAsMultipartAsync(provider).Wait();

                foreach (var file in provider.FileData)
                {
                    using (var fileStream = new FileStream(file.LocalFileName, FileMode.Open, FileAccess.Read, FileShare.Read, 1024 * 1024))
                    {
                        var bytes = fileStream.ReadAllBytes();
                        var package = packageBuilder.Build(bytes);
                        blobService.Upload(GetFilepath(package), bytes);
                        packageService.Add(package);
                    }
                }
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception exception)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}