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

namespace Fluent.Architecture.Filters
{
    public class FluentAuthorizationFilter : IAuthorizationFilter
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

            if (Setup.Config.Config.JwtInfo != null)
            {
                JWTOnFluentAuthorizationFilter(context);
            }

            OnFluentAuthorizationFilter(context);
        }

        protected virtual void JWTOnFluentAuthorizationFilter(AuthorizationFilterContext context)
        {
            var tokenRequest = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer", "").Trim();
            tokenRequest = string.IsNullOrWhiteSpace(tokenRequest) ? context.HttpContext.Request.Cookies["Authorization"]?.Replace("Bearer", "")?.Trim() : tokenRequest;
            if (string.IsNullOrWhiteSpace(tokenRequest) || tokenRequest == "undefined" && tokenRequest == "null")
            {
                Forbidden(context);
            }
            else
            {
                var par = SigningConfigurations.GetTokenValidationParameters();
                var handler = new JwtSecurityTokenHandler();
                try
                {
                    context.HttpContext.User = handler.ValidateToken(tokenRequest, par, out SecurityToken tok);
                }
                catch (Exception)
                {
                    Forbidden(context);
                }
            }
        }

        protected virtual void OnFluentAuthorizationFilter(AuthorizationFilterContext context)
        {
        }

        private void Forbidden(AuthorizationFilterContext context)
        {
            context.Result = new ForbidResult();
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return;
        }
    }
}