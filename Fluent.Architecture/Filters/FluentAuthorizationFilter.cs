using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Reflection;
using dn32.infra.extensoes;

namespace dn32.infra.Filters
{
    public class DnAuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var action = context?.ActionDescriptor as ControllerActionDescriptor;
            if (action?.ControllerTypeInfo.GetCustomAttribute<AllowAnonymousAttribute>() != null)
            {
                return;
            }

            if (action?.MethodInfo.GetCustomAttribute<AllowAnonymousAttribute>() != null)
            {
                return;
            }

            if (Setup.ConfiguracoesGlobais.InformacoesDoJWT != null)
            {
                JWTOnDnAuthorizationFilter(context);
            }

            OnDnAuthorizationFilter(context);
        }

        protected virtual void JWTOnDnAuthorizationFilter(AuthorizationFilterContext context)
        {
            var tokenRequest = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer", "").Trim();
            tokenRequest = string.IsNullOrWhiteSpace(tokenRequest) ? context.HttpContext.Request.Query["Authorization"].ToString()?.Replace("Bearer", "")?.Trim() : tokenRequest;
            tokenRequest = string.IsNullOrWhiteSpace(tokenRequest) ? context.HttpContext.Request.Cookies["Authorization"]?.Replace("Bearer", "")?.Trim() : tokenRequest;

            if (string.IsNullOrWhiteSpace(tokenRequest) || tokenRequest == "undefined" && tokenRequest == "null")
            {
                Forbidden(context, "An authentication token is EhRequerido");
            }
            else
            {
                var par = SigningConfigurations.GetTokenValidationParameters();
                var handler = new JwtSecurityTokenHandler();
                try
                {
                    context.HttpContext.User = handler.ValidateToken(tokenRequest, par, out SecurityToken tok);
                }
                catch (Exception ex)
                {
                    Forbidden(context, ex.Message);
                }
            }
        }

        protected virtual void OnDnAuthorizationFilter(AuthorizationFilterContext context)
        {
        }

        private void Forbidden(AuthorizationFilterContext context, string message)
        {
            ContentResult content = new ContentResult
            {
                ContentType = "application/json",
                Content = message.SerializarParaDnJson()
            };

            context.Result = content;
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return;
        }
    }
}