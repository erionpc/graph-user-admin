using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace B2CUserAdmin.Shared.Paging
{
    public class PaginatedResponse<T> where T : class, IEnumerable
    {
        public T? Body { get; set; }
        public string? NextPageLink { get; set; }
        public IDictionary<string, object>? AdditionalData { get; set; }

        public PaginatedResponse()
        {

        }

        public PaginatedResponse(T? body, string? nextPageLink, IDictionary<string, object>? additionalData)
        {
            Body = body;
            NextPageLink = nextPageLink;
            AdditionalData = additionalData;
        }
    }
}