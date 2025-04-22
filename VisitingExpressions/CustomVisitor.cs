using System.Linq.Expressions;
using System.Text;

namespace VisitingExpressions
{
    public class CustomVisitor : ExpressionVisitor
    {
        private StringBuilder Sql = new();

        public Expression Modify(Expression expression)
        {
            return Visit(expression);
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            var pIndex = 0;

            switch (node.Body.NodeType)
            {
                case ExpressionType.Equal:
                    VisitBinary((BinaryExpression)node.Body);
                    break;
            }
            return node;
        }

        protected override Expression VisitBinary(BinaryExpression b)
        {
            if (b.NodeType == ExpressionType.Equal)
            {
                Expression left = this.Visit(b.Left);
                Sql.Append("=");
                Expression right = this.Visit(b.Right);
            }

            return b;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            Sql.Append("@" + node.Name);
            return base.VisitParameter(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            Sql.Append(node.Value);
            return base.VisitConstant(node);
        }
        public string BuildSql()
        {
            return Sql.ToString();
        }
    }
}