using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.OData.Routing;
using System.Web.Http.OData.Routing.Conventions;

namespace TinyFeed.OData.Routing.Conventions
{
    public class CompositeKeyPropertyRoutingConvention : PropertyRoutingConvention
    {
        public override string SelectAction(ODataPath odataPath, HttpControllerContext controllerContext, ILookup<string, HttpActionDescriptor> actionMap)
        {
            var action = base.SelectAction(odataPath, controllerContext, actionMap);

            if (action != null)
            {
                controllerContext.RouteData.DecomposeKey();
            }

            return action;
        }
    }
}
