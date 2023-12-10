using System.Web;

namespace GraphUserAdmin.Shared.Paging
{
    public class PaginatedSearchRequest
    {
        public int? PageSize { get; set; }
        public Uri? PagingLink { get; set; }

        public virtual Dictionary<string, string> ToDictionary()
        {
            Dictionary<string, string> queryParameters = [];

            if (PageSize.HasValue)
                queryParameters.Add(nameof(PageSize), PageSize.ToString()!);

            if (PagingLink is not null)
                queryParameters.Add(nameof(PagingLink), HttpUtility.UrlEncode(PagingLink.OriginalString));

            return queryParameters;
        }
    }
}
