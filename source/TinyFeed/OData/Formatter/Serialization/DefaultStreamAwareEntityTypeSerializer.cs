﻿using System;
using System.Web.Http.OData;
using System.Web.Http.OData.Formatter.Serialization;
using Microsoft.Data.OData;

namespace TinyFeed.OData.Formatter.Serialization
{
    public abstract class DefaultStreamAwareEntityTypeSerializer<T> : ODataEntityTypeSerializer where T : class
    {
        protected DefaultStreamAwareEntityTypeSerializer(ODataSerializerProvider serializerProvider)
            : base(serializerProvider)
        {
        }

        public override ODataEntry CreateEntry(SelectExpandNode selectExpandNode, EntityInstanceContext entityInstanceContext)
        {
            var entry = base.CreateEntry(selectExpandNode, entityInstanceContext);

            var instance = entityInstanceContext.EntityInstance as T;

            if (instance != null)
            {
                entry.MediaResource = new ODataStreamReferenceValue
                {
                    ContentType = ContentType,
                    ReadLink = BuildLinkForStreamProperty(instance, entityInstanceContext)
                };
            }
            return entry;
        }

        public virtual string ContentType
        {
            get { return "application/octet-stream"; }
        }

        public abstract Uri BuildLinkForStreamProperty(T entity, EntityInstanceContext entityInstanceContext);
    }
}
