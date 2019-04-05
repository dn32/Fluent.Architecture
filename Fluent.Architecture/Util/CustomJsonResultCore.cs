#if !NET461

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
#endif