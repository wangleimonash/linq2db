using System;
using System.Linq.Expressions;

namespace LinqToDB.Linq.Builder
{
	using SqlQuery;

	class SelectExpressionBuilder : ExpressionBuilderBase
	{
		public static QueryExpression Translate(QueryExpression qe, MethodCallExpression expression)
		{
			if (expression.Arguments.Count == 2)
			{
				var expr = expression.Arguments[1];

				while (expr.NodeType == ExpressionType.Quote)
					expr = ((UnaryExpression)expr).Operand;

				var l = (LambdaExpression)expr;

				if (l.Parameters.Count == 1 && l.Body == l.Parameters[0])
					return qe;
			}

			return qe.AddBuilder(new SelectExpressionBuilder(expression));
		}

		SelectExpressionBuilder(Expression expression) : base(expression)
		{
		}

		public override Expression BuildQueryExpression<T>(QueryBuilder<T> builder)
		{
			throw new NotImplementedException();
		}

		public override void BuildQuery<T>(QueryBuilder<T> builder)
		{
			throw new NotImplementedException();
		}

		ProjectionSqlQueryBuilder _sqlQueryBuilder;

		public override SelectQuery BuildSql<T>(QueryBuilder<T> builder, SelectQuery selectQuery)
		{
			_sqlQueryBuilder = new ProjectionSqlQueryBuilder(builder, selectQuery);
			return _sqlQueryBuilder.SelectQuery;
		}
	}
}