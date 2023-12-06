using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace B2CUserAdmin.UI.Extensions
{
    public static class HttpClientExtensions
    {
        public static Task<TValue?> GetFromJsonAsync<TValue>(this HttpClient client, string? requestUri, Dictionary<string, string> queryParameters, JsonSerializerOptions? jsonSerializerOptions = null, CancellationToken cancellationToken = default)
        {
            // If there are no query parameters then call the vanilla GetFromJsonAsync
            if (!queryParameters.Any()) 
                return client.GetFromJsonAsync<TValue>(requestUri, jsonSerializerOptions, cancellationToken);

            var query = HttpUtility.ParseQueryString(string.Empty);
            foreach (var item in queryParameters)
            {
                query[item.Key] = item.Value;
            }

            // If the client has a base address use it, otherwise use the requestUri
            UriBuilder builder = client.BaseAddress != null ? new(client.BaseAddress + requestUri) : new(requestUri ?? "");

            // the requestUri might already have query parameters in it, join them then remove any tailing '&'
            builder.Query = string.Join('&', builder.Query, query.ToString()).Trim('&');

            // replace the requestUri with the new one, containing query parameters.
            // if the host is empty, we only want the PathAndQuery
            requestUri = string.IsNullOrWhiteSpace(builder.Host) ? builder.ToString() : builder.Uri.PathAndQuery;

            return client.GetFromJsonAsync<TValue>(requestUri, jsonSerializerOptions, cancellationToken);
        }
    }
}
