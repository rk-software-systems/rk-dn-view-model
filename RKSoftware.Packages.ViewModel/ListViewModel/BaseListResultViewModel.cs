using System;
using System.Collections.Generic;

namespace RKSoftware.Packages.ViewModel
{
    /// <summary>
    /// This view model is used as a base list result view model
    /// </summary>
    /// <typeparam name="T">List item type</typeparam>
    public class BaseListResultViewModel<T>
        where T : class
    {
        private bool prev;
        private bool next;

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
                    queryString = "?PageNumber=1&" + queryString;
                }

                if (!queryString.Contains("PageSize", StringComparison.Ordinal))
                {
                    var i = queryString.IndexOf('&');

                    if (i != -1)
                    {
                        queryString = queryString.Insert(i + 1, "PageSize=10&");
                    }

                    i = queryString.IndexOf('&', i + 1);

                    if (i != -1 && queryString.Length == i)
                    {
                        queryString.Remove(i);
                    }
                }
            }

            var link = path + queryString;

            if (next)
            {
                Next = link.Replace($"{nameof(requestModel.PageNumber)}={PageNumber}",
                    $"{nameof(requestModel.PageNumber)}={PageNumber + 1}");
            }

            if (prev)
            {
                Prev = link.Replace($"{nameof(requestModel.PageNumber)}={PageNumber}",
                    $"{nameof(requestModel.PageNumber)}={PageNumber - 1}");
            }
        }
    }
}