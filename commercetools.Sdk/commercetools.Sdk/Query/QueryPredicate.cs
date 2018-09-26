﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.Domain
{
    public class QueryPredicate<T>
    {
        public QueryPredicate(Expression<Func<T, bool>> func)
        {

        }
    }

    public static class QueryExtensions
    {
        public static bool In<T>(this T source, params T[] values)
        {
            return values.Contains(source);
        }
    }

}