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

        [HttpGet, HttpHead]
        public HttpResponseMessage DownloadPackage(string id, string version = null)
        {
            var package = version == null
                ? packageService.FindLatestPackage(id)
                : packageService.FindPackage(id, version);

            if (package == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            var response = Request.CreateResponse(HttpStatusCode.OK);

            if (Request.Method == HttpMethod.Get)
            {
                response.Content = new StreamContent(blobService.Download(GetBlobScope(package), package.BlobId));
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
                FileName = GetFilename(package),
                CreationDate = package.Created,
                ModificationDate = package.LastUpdated
            };

            return response;
        }

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
                    using (var fileStream = new FileStream(file.LocalFileName, FileMode.Open, FileAccess.Read, FileShare.Read, 1024*1024))
                    {
                        var bytes = fileStream.ReadAllBytes();
                        Package package;
                        if (!packageBuilder.TryBuild(bytes, out package))
                        {
                            return new HttpResponseMessage(HttpStatusCode.BadRequest);
                        }

                        blobService.Upload(GetBlobScope(package), package.BlobId, bytes);
                        packageService.Add(package);
                    }
                }
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        private static string GetBlobScope(Package package)
        {
            return Path.Combine(package.Id, package.Version);
        }

        private static string GetFilename(Package package)
        {
            return string.Format("{0}.{1}{2}", package.Id, package.Version, Constants.PackageExtension);
        }
    }
}