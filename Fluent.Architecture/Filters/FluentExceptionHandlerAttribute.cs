using dn32.infra.nucleo.erros_de_validacao;
using dn32.infra.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using dn32.infra.extensoes;
using dn32.infra.nucleo.insonsistencias;
using dn32.infra.nucleo.modelos;

namespace dn32.infra.Filters
{
    public class DnExceptionHandlerAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext?.Exception == null) { return; }

            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();

            if (filterContext.Exception?.InnerException is DbUpdateException exception0)
            {
                filterContext.Exception = exception0.InnerException;
            }

            while (filterContext.Exception is TargetInvocationException exception1)
            {
                filterContext.Exception = exception1.InnerException;
            }

            if (filterContext.Exception is TargetInvocationException exception2)
            {
                filterContext.Exception = exception2.InnerException;
            }

            if (filterContext.Exception is ContextDnValidationException exception)
            {
                var inconsistencies = exception.Inconsistencies.Select(inconsistence =>
                {
                    if (inconsistence is DnCampoDeTelaErroDeValidacao field)
                    {
                        return new DnInconsistenciaDeCampoDeTela
                        {
                            Campo = field.Campo,
                            Mensagem = field.Mensagem,
                            ChaveDeGlobalizacao = field.ChaveDeGlobalizacao,
                            NomeDaPropriedade = field.NomeDaPropriedade,
                            DnErroDeValidacao = inconsistence
                        };
                    }
                    else if (inconsistence is DnPropriedadeErroDeValidacao prop)
                    {
                        return new DnInconsistenciaDePropriedade
                        {
                            Mensagem = prop.Mensagem,
                            ChaveDeGlobalizacao = prop.ChaveDeGlobalizacao,
                            NomeDaPropriedade = prop.NomeDaPropriedade,
                            DnErroDeValidacao = inconsistence
                        };
                    }
                    else
                    {
                        return new DnInconsistencia
                        {
                            Mensagem = inconsistence.Mensagem,
                            ChaveDeGlobalizacao = inconsistence.ChaveDeGlobalizacao,
                            DnErroDeValidacao = inconsistence
                        };
                    }
                })
                .ToList();

                inconsistencies.ForEach(GetGlobalization);

                // filterContext.Result =
                var result = new DnRetornoDeValidacao
                {
                    Inconsistencias = inconsistencies,
                    Mensagem = exception.Message,
                    EhErroDeValidacao = true
                };

                ContentResult content = new ContentResult
                {
                    ContentType = "application/json",
                    Content = result.SerializarParaDnJson()
                };

                filterContext.Result = content;
                filterContext.HttpContext.Response.StatusCode = 422;
            }
            else
            {
                if (filterContext.Exception == null) { return; }
                var stackTrace = new StackTrace(filterContext.Exception, true);
                var frame = stackTrace.GetFrame(0);
                var line = frame?.GetFileLineNumber() ?? -1;

                var result = new
                {
                    Error = true,
                    filterContext.Exception?.Message,
                    stackTrace,
                    line
                };

                ContentResult content = new ContentResult
                {
                    ContentType = "application/json",
                    Content = result.SerializarParaDnJson()
                };

                filterContext.Result = content;
                filterContext.HttpContext.Response.StatusCode = 500;
            }
        }

        public virtual void GetGlobalization(DnInconsistencia inconsistence)
        {
        }
    }
}