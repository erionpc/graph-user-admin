using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphUserAdmin.Shared.Paging
{
    public class PaginatedSearchRequest
    {
        public int? PageSize { get; set; }
        public string? PagingToken { get; set; }

        public virtual Dictionary<string, string> ToDictionary()
        {
            Dictionary<string, string> queryParameters = new();

            if (PageSize.HasValue)
                queryParameters.Add(nameof(PageSize), PageSize.ToString()!);

            if (!string.IsNullOrWhiteSpace(PagingToken))
                queryParameters.Add(nameof(PagingToken), PagingToken);

            return queryParameters;
        }
    }
}
