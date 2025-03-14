﻿namespace RKSoftware.Packages.ViewModel.Extensions;

/// <summary>
/// This class contains Base List request common Extension methods
/// </summary>
public static class BaseListRequestExtensions
{
    /// <summary>
    /// Apply list paging and sorting
    /// </summary>
    /// <typeparam name="T">List domain entity type</typeparam>
    /// <param name="source">Domain Entities queryable source</param>
    /// <param name="listRequest">List model to apply on Source</param>
    /// <returns>Queryable with applied list request model</returns>
    public static IQueryable<T> ApplyList<T>(this IQueryable<T> source, BaseListRequestViewModel listRequest)
    {
        return source.ApplyList(listRequest, true);
    }

    /// <summary>
    /// Apply list paging and sorting
    /// </summary>
    /// <typeparam name="T">List domain entity type</typeparam>
    /// <param name="source">Domain Entities queryable source</param>
    /// <param name="listRequest">List model to apply on Source</param>
    /// <param name="isSorting">Using a sorting process</param>
    /// <returns>Queryable with applied list request model</returns>
    public static IQueryable<T> ApplyList<T>(this IQueryable<T> source, BaseListRequestViewModel listRequest, bool isSorting)
    {
        if (isSorting)
        {
            source = source.ApplySorting(listRequest);
        }

        return source.ApplyPaginations(listRequest);
    }

    /// <summary>
    /// Apply list paging
    /// </summary>
    /// <typeparam name="T">List domain entity type</typeparam>
    /// <param name="source">Domain Entities queryable source</param>
    /// <param name="listRequest">List model to apply on Source</param>
    /// <returns></returns>
    public static IQueryable<T> ApplyPaginations<T>(this IQueryable<T> source, BaseListRequestViewModel listRequest)
    {
        ArgumentNullException.ThrowIfNull(listRequest, nameof(listRequest));

        var pageSize = listRequest.PageSize == int.MaxValue ? int.MaxValue : listRequest.PageSize + 1;

        return source
            .Skip((listRequest.PageNumber - 1) * listRequest.PageSize)
            .Take(pageSize);
    }

    /// <summary>
    /// Apply list sorting
    /// </summary>
    /// <typeparam name="T">List domain entity type</typeparam>
    /// <param name="source">Domain Entities queryable source</param>
    /// <param name="listRequest">List model to apply on Source</param>
    /// <returns></returns>
    public static IQueryable<T> ApplySorting<T>(this IQueryable<T> source, BaseListRequestViewModel listRequest)
    {
        ArgumentNullException.ThrowIfNull(listRequest, nameof(listRequest));

        string? sortField;

        if (!string.IsNullOrEmpty(listRequest.SortField))
        {
            sortField = listRequest.SortField;
        }
        else
        {
            var myType = typeof(T);

            sortField = myType.GetProperties()
                .FirstOrDefault()
                ?.Name;
        }

        if (!string.IsNullOrEmpty(sortField))
        {
            if (listRequest.SortAscending)
            {
                source = source.OrderByProperty(sortField);
            }
            else
            {
                source = source.OrderByPropertyDescending(sortField);
            }
        }

        return source;
    }


    /// <summary>
    /// Validated if particular Domain Entity type could be used with BaseList Request object
    /// </summary>
    /// <typeparam name="T">Domain entity Type</typeparam>
    /// <param name="listRequest">Base List request object</param>
    /// <returns>False in case it is not possible to apply this list object on provided Entity type.</returns>
    public static bool IsDomainSortingValid<T>(this BaseListRequestViewModel listRequest)
    {
        ArgumentNullException.ThrowIfNull(listRequest, nameof(listRequest));

        if (string.IsNullOrEmpty(listRequest.SortField))
        {
            return true;
        }

        return typeof(T).GetProperties()
            .Any(x => string.Equals(x.Name, listRequest.SortField,
                StringComparison.OrdinalIgnoreCase));
    }
}