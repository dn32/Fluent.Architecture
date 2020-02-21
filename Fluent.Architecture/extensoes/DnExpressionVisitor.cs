using System.Collections.Generic;
using System.Linq.Expressions;

namespace dn32.infra.nucleo.extensoes
{
    internal class DnExpressionVisitor : ExpressionVisitor
    {
        public Dictionary<Expression, Expression> Dicionario = new Dictionary<Expression, Expression>();
    }
}