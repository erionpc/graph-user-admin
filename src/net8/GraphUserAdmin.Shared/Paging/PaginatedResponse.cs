using System.Collections;

namespace GraphUserAdmin.Shared.Paging
{
    public class PaginatedResponse<T> where T : class
    {
        public List<T> Data { get; set; } = [];
        public Uri? NextPagingLink { get; set; }

        public PaginatedResponse<T2> CloneFromThis<T2>(IEnumerable<T2> data) where T2 : class =>
            new()
            {
                Data = data.ToList(),
                NextPagingLink = NextPagingLink
            };
    }
}