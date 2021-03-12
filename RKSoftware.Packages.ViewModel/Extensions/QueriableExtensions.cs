using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace RKSoftware.Packages.ViewModel.Extensions
{
    /// <summary>
    /// Queriable Extensions
    /// </summary>
    public static class QueriableExtensions
    {
        private static readonly MethodInfo OrderByDescendingMethod =
            typeof(Queryable)
                .GetMethods()
                .Where(method => method.Name == "OrderByDescending")
                .Single(method => method.GetParameters()
                    .Length == 2);

        private static readonly MethodInfo OrderByMethod =
            typeof(Queryable)
                .GetMethods()
                .Where(method => method.Name == "OrderBy")
                .Single(method => method.GetParameters()
                    .Length == 2);

        /// <summary>
        /// Order By Property
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <returns></returns>
        public static IQueryable<TSource> OrderByProperty<TSource>(this IQueryable<TSource> source,
            string propertyName)
        {
            var parameter = Expression.Parameter(typeof(TSource), typeof(TSource).Name);
            Expression orderByProperty = Expression.Property(parameter, propertyName);

            var lambda = Expression.Lambda(orderByProperty, parameter);
            var genericMethod = OrderByMethod.MakeGenericMethod(typeof(TSource), orderByProperty.Type);

            var ret = genericMethod.Invoke(null, new object[] {source, lambda});
            return (IQueryable<TSource>) ret;
        }

        /// <summary>
        /// Order By Property Descending
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <returns></returns>
        public static IQueryable<TSource> OrderByPropertyDescending<TSource>(this IQueryable<TSource> source,
            string propertyName)
        {
            var parameter = Expression.Parameter(typeof(TSource), typeof(TSource).Name);
            Expression orderByProperty = Expression.Property(parameter, propertyName);

            var lambda = Expression.Lambda(orderByProperty, parameter);

            var genericMethod =
                OrderByDescendingMethod.MakeGenericMethod(typeof(TSource), orderByProperty.Type);

            var ret = genericMethod.Invoke(null, new object[] {source, lambda});
            return (IQueryable<TSource>) ret;
        }
    }
}