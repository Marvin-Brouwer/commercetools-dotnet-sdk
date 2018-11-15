﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.Linq
{
    public class AttributePredicateVisitorConverter : ICartPredicateVisitorConverter
    {
        public bool CanConvert(Expression expression)
        {
            if (expression is MethodCallExpression methodCallExpression)
            {             
                if (IsMethodNameAllowed(methodCallExpression) && IsValidMethodCaller(methodCallExpression))
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsMethodNameAllowed(MethodCallExpression expression)
        {
            return expression.Method.Name == "Any";
        }

        private bool IsValidMethodCaller(MethodCallExpression expression)
        {
            Expression callerExpression = expression.Arguments[0];
            if (callerExpression is MethodCallExpression methodCallExpression)
            {
                if (methodCallExpression.Method.Name == "Attributes")
                {
                    return true;
                }
            }
            return false;
        }

        public ICartPredicateVisitor Convert(Expression expression, ICartPredicateVisitorFactory cartPredicateVisitorFactory)
        {
            MethodCallExpression methodCallExpression = expression as MethodCallExpression;
            if (expression == null)
            {
                throw new ArgumentException();
            }
            var innerExpression = methodCallExpression.Arguments[1];
            if (innerExpression is LambdaExpression lambdaExpression)
            {
                var attributeExpression = lambdaExpression.Body;
                if (attributeExpression.NodeType == ExpressionType.And || attributeExpression.NodeType == ExpressionType.AndAlso)
                {
                    ICartPredicateVisitor attributeValuePredicateVisitor = cartPredicateVisitorFactory.Create(((BinaryExpression)attributeExpression).Right);
                    string attributeName = GetAttributeName(((BinaryExpression)attributeExpression).Left);
                    Accessor parentAccessor = cartPredicateVisitorFactory.Create(methodCallExpression.Arguments[0]) as Accessor;
                    ConstantPredicateVisitor constantPredicateVisitor = new ConstantPredicateVisitor(attributeName);
                    Accessor accessor = new Accessor(constantPredicateVisitor, parentAccessor);
                    attributeValuePredicateVisitor.AppendAccessor(accessor);
                    return attributeValuePredicateVisitor;
                }
            }      

            throw new NotSupportedException();
        }

        private string GetAttributeName(Expression expression)
        {
            if (expression is BinaryExpression nameExpression)
            {
                if (nameExpression.Left is MemberExpression memberExpression && memberExpression.Member.Name == "Name")
                {
                    return nameExpression.Right.ToString().Replace("\"", "");
                }
            }
            throw new NotSupportedException();
        }
    }
}
