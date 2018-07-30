using System;
using System.Collections.Generic;

namespace System.Linq
{
    public static class LinqExtentions
    {
        public static IEnumerable<T> AsPage<T>(this IEnumerable<T> itens, int skip = 0, int take = 20){
            if (itens ==null)
            {
                throw new ArgumentNullException(nameof(itens));
            }

            return itens.Skip(skip).Take(take);
        }
    }
}