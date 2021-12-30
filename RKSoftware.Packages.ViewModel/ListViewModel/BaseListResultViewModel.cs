using System;
using System.Collections.Generic;
using System.Linq;

namespace RKSoftware.Packages.ViewModel
{
    /// <summary>
    /// This view model is used as a base list result view model
    /// </summary>
    /// <typeparam name="T">List item type</typeparam>
    public class BaseListResultViewModel<T>
        where T : class
    {
        public bool NextPage { get; private set; }

        /// <summary>
        /// List item data
        /// </summary>
        public List<T> Data { get; set; }

        /// <summary>
        /// Page number
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Page size
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Previous page
        /// </summary>
        public string Prev { get; set; }

        /// <summary>
        /// Next page
        /// </summary>
        public string Next { get; set; }

        public void CheckAndSetNext()
        {
            if (Data.Count > PageSize)
            {
                NextPage = true;
                Data = Data
                    .Take(PageSize)
                    .ToList();
            }
        }

        /// <summary>
        /// Set Reference to link
        /// </summary>
        /// <param name="path"></param>
        /// <param name="queryString"></param>
        /// <param name="requestModel"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void SetRef(string path, string queryString, BaseListRequestViewModel requestModel)
        {
            if (requestModel is null)
            {
                throw new ArgumentNullException(nameof(requestModel));
            }

            if (string.IsNullOrEmpty(queryString))
            {
                queryString = "?PageNumber=1&PageSize=10";
            }
            else
            {
                if (!queryString.Contains("PageNumber", StringComparison.Ordinal))
                {
                    queryString += "&PageNumber=1";
                }

                if (!queryString.Contains("PageSize", StringComparison.Ordinal))
                {
                    queryString += "&PageSize=10";
                }
            }

            var link = path + queryString;

            if (NextPage)
            {
                Next = link.Replace($"{nameof(requestModel.PageNumber)}={PageNumber}",
                    $"{nameof(requestModel.PageNumber)}={PageNumber + 1}", StringComparison.OrdinalIgnoreCase);
            }

            if (PageNumber != 1)
            {
                Prev = link.Replace($"{nameof(requestModel.PageNumber)}={PageNumber}",
                    $"{nameof(requestModel.PageNumber)}={PageNumber - 1}", StringComparison.OrdinalIgnoreCase);
            }
        }
    }

}