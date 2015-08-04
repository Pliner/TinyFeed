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

            var entity = builder.EntitySet<ODataPackage>("Packages");
            entity.EntityType.HasKey(pkg => pkg.Id);
            entity.EntityType.HasKey(pkg => pkg.Version);

            var findPackagesAction = builder.Action("FindPackagesById");
            findPackagesAction.Parameter<string>("id");
            findPackagesAction.ReturnsCollectionFromEntitySet<ODataPackage>("Packages");

            model = builder.GetEdmModel();
            model.SetHasDefaultStream(model.FindDeclaredType(typeof(ODataPackage).FullName) as IEdmEntityType, hasStream: true);
        }
    }
}