﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.Linq
{
    public class UnaryPredicateVisitor : ICartPredicateVisitor
    {
        private ICartPredicateVisitor operand;
        private string operatorName; 

        public UnaryPredicateVisitor(ICartPredicateVisitor operand, string operatorName)
        {
            this.operand = operand;
            this.operatorName = operatorName;
        }

        public void AppendAccessor(Accessor accessor)
        {
            if (operand != null)
            {
                operand.AppendAccessor(accessor);
            }
            else
            {
                operand = accessor;
            }
        }

        public string Render()
        {
            return $"{this.operand?.Render()} {operatorName}";
        }


    }
}
