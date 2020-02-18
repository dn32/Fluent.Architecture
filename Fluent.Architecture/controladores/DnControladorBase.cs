using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[assembly: InternalsVisibleTo(@"dn32.infra.Test, PublicKey=00240000048000009400000006020000002400005253413100040000010001006952445a097c4a77d392a65e93eb0384ea650700e6777d7ac51223f9f096134bd787cf032b4b92b3670dc0c08cce91bcc3b66639a64728eb6af0c0b269bc4defc1fd7b7014ecde709304051d825377bd9839f843a8d6a23220b63c66d57631d00196ced300b03fb233f075e89a68bea175d2fa1fec154b628d5362f0513cc5ec")]
[assembly: InternalsVisibleTo(@"dn32.infra.Sample.Test, PublicKey=0024000004800000940000000602000000240000525341310004000001000100e59e0f13e7bf43e7c1536d8a75c6e67c04a08206abd001a65986bcb6375c07c7b19a8598260be4b21898327679b62736b08e17707f814de3bdb9b1285b85e28238873f151f6add37fa4317ed18446b0f28d9aaacc1f4c7cabb2205e07dac7a2bedb3456e07b7b4a367ca0ed7e42c73516f4422e6cb849e5ad923e4f37d02e1d4")]
namespace dn32.infra.nucleo.controladores
{
    [SuppressMessage("ReSharper", "IdentifierTypo")]
    public abstract class DnControladorBase : Controller
    {
        private HttpContext localHttpContext;

        public new HttpContext HttpContext => this.localHttpContext ?? base.HttpContext;

        public new IPrincipal User => this.HttpContext.User;

        public void SetLocalHttpContext(HttpContext httpContext)
        {
            this.localHttpContext = httpContext;
        }
    }
}
