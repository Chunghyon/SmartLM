using System.Linq.Expressions;

namespace FaceWebServer.Utility.Extend
{
    /// <summary>
    /// 建立新表达式
    /// </summary>
    internal class NewExpressionVisitor : ExpressionVisitor
    {
        public ParameterExpression _NewParameter { get; private set; }
        public NewExpressionVisitor(ParameterExpression param)
        {
            _NewParameter = param;
        }
        public Expression Replace(Expression exp)
        {
            return Visit(exp);
        }
        protected override Expression VisitParameter(ParameterExpression node)
        {
            return _NewParameter;
        }
    }
}
