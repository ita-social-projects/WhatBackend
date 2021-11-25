using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CharlieBackend.Data.Helpers
{
    public static class ExtensionMethods
    {
        public static void TryUpdateManyToMany<T>(this DbSet<T> dbSet,
                                                 IEnumerable<T> currentItems,
                                                 IEnumerable<T> newItems)
            where T : BaseEntity
        {
            dbSet.RemoveRange(currentItems);
            dbSet.AddRange(newItems);
        }

        //public static IEnumerable<T> Except<T, TKey>(this IEnumerable<T> items, 
        //                                            IEnumerable<T> other,
        //                                            Func<T, TKey> getKeyFunc)
        //{
        //    return items
        //        .GroupJoin(other, getKeyFunc, getKeyFunc, (item, tempItems)
        //              => new { item, tempItems })
        //        .SelectMany(t => t.tempItems.DefaultIfEmpty(), (t, temp) 
        //              => new { t, temp })
        //        .Where(t => ReferenceEquals(null, t.temp) 
        //              || t.temp.Equals(default(T)))
        //        .Select(t => t.t.item);
        //}
    }
}
