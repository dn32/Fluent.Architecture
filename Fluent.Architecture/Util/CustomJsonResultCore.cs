using Fluent.Architecture.Core.Attributes;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Fluent.Architecture.Util
{
    public class CustomJsonResult : JsonResult
    {
        public CustomJsonResult(object value) : base(value)
        {
        }

        public CustomJsonResult(object value, JsonSerializerSettings serializerSettings) : base(value, serializerSettings)
        {
        }

        public static void AdjustColumns(List<FluentJsonPropertyAttribute> props, int row)
        {
            var sum = props.Sum(y => y.lGrid);
            var count = props.Count();
            int i = 0;

            while (sum < 12)
            {
                props[i].lGrid++;
                sum = props.Sum(y => y.lGrid);
                if (i + 1 == count) { i = 0; } else { i++; }
            }
        }

        // Todo - Verificar se teremos necessidade de algum tratamento no core

        //private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        //{
        //    Formatting = Formatting.Indented,
        //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        //};
        //public override void ExecuteResult(ControllerContext context)
        //{
        //    if (JsonRequestBehavior == JsonRequestBehavior.DenyGet && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
        //    {
        //        throw new InvalidOperationException("GET request not allowed");
        //    }

        //    var response = context.HttpContext.Response;

        //    response.ContentType = !string.IsNullOrEmpty(ContentType) ? ContentType : "application/json";

        //    if (ContentEncoding != null)
        //    {
        //        response.ContentEncoding = ContentEncoding;
        //    }

        //    if (Data == null)
        //    {
        //        return;
        //    }

        //    response.Write(JsonConvert.SerializeObject(Data, Settings));
        //}
    }
}