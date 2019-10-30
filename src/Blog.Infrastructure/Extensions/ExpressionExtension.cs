using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Blog.Infrastructure.Extensions
{
    public static class ExpressionExtension
    {
        public static MemberInfo GetMemberInfo<TEntity, TMember>(this Expression<Func<TEntity, TMember>> expression)
        {
            if (expression.NodeType != ExpressionType.Lambda)
            {
                throw new ArgumentException(nameof(expression));
            }
            var lambda = (LambdaExpression)expression;
            var memberExpression = ExtractMemberExpression(lambda.Body);
            if (memberExpression == null)
            {
                throw new ArgumentException(nameof(memberExpression));
            }
            return memberExpression.Member;
        }

        private static MemberExpression ExtractMemberExpression(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.MemberAccess:
                    return (MemberExpression)expression;
                case ExpressionType.Convert:
                {
                    var operand = ((UnaryExpression)expression).Operand;
                    return ExtractMemberExpression(operand);
                }
                default:
                    return null;
            }
        }

    }
}