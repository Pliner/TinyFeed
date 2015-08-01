using System;
using System.Web.Http.OData.Builder;
using Microsoft.Data.Edm;
using Microsoft.Data.OData;
using TinyFeed.Models;

namespace TinyFeed
{
    public class NuGetWebApiODataModelBuilder
    {
        private IEdmModel model;

        public IEdmModel Model
        {
            get
            {
                if (model == null)
                {
                    throw new InvalidOperationException("Must invoke Build method before accessing Model.");
                }
                return model;
            }
        }

        public void Build()
        {
            var builder = new ODataConventionModelBuilder();

            var entity = builder.EntitySet<V2FeedPackage>("Packages");
            entity.EntityType.HasKey(pkg => pkg.Id);
            entity.EntityType.HasKey(pkg => pkg.Version);

            var searchAction = builder.Action("Search");
            searchAction.Parameter<string>("searchTerm");
            searchAction.Parameter<string>("targetFramework");
            searchAction.Parameter<bool>("includePrerelease");
            searchAction.ReturnsCollectionFromEntitySet<V2FeedPackage>("Packages");

            var findPackagesAction = builder.Action("FindPackagesById");
            findPackagesAction.Parameter<string>("id");
            findPackagesAction.ReturnsCollectionFromEntitySet<V2FeedPackage>("Packages");

            var getUpdatesAction = builder.Action("GetUpdates");
            getUpdatesAction.Parameter<string>("packageIds");
            getUpdatesAction.Parameter<bool>("includePrerelease");
            getUpdatesAction.Parameter<bool>("includeAllVersions");
            getUpdatesAction.Parameter<string>("targetFrameworks");
            getUpdatesAction.Parameter<string>("versionConstraints");
            getUpdatesAction.ReturnsCollectionFromEntitySet<V2FeedPackage>("Packages");

            model = builder.GetEdmModel();
            model.SetHasDefaultStream(model.FindDeclaredType(typeof(V2FeedPackage).FullName) as IEdmEntityType, hasStream: true);
        }
    }
}