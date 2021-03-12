namespace RKSoftware.Packages.ViewModel
{
    /// <summary>
    /// This view model is used as a base model for List requests
    /// </summary>
    public class BaseListRequestViewModel
    {
        /// <summary>
        /// Page Number
        /// </summary>
        public int PageNumber { get; init; } = 1;

        /// <summary>
        /// Page size
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// Sort result list using particular field
        /// </summary>
        public string SortField { get; set; }

        /// <summary>
        /// Sort ascending / descending
        /// </summary>
        public bool SortAscending { get; set; }
    }
}