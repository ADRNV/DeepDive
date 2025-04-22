using System.Linq.Expressions;
using VisitingExpressions;

var nodes = new List<Expression>().AsReadOnly();

var visitor = new CustomVisitor();

Expression expression = Expression.MakeBinary(
    ExpressionType.Equal,
    Expression.Constant(0), 
    Expression.Parameter(typeof(int)));

LambdaExpression labelExpression = Expression.Lambda<Func<int, bool>>(expression, [Expression.Parameter(typeof(int))]);

visitor.Modify(labelExpression);

Console.WriteLine(visitor.BuildSql());