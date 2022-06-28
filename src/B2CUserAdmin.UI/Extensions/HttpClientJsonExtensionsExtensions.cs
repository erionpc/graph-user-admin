using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace System.Net.Http.Json
{
    public static class HttpClientJsonExtensionsExtensions
    {
        public static Task<TValue?> GetFromJsonAsync<TValue>(this HttpClient client, string? requestUri, IDictionary<string, string> queryParameters, JsonSerializerOptions? jsonSerializerOptions = null, CancellationToken cancellationToken = default)
        {
            // If there are no query parameters then call the vanilla GetFromJsonAsync.
            if (!queryParameters.Any()) return client.GetFromJsonAsync<TValue>(requestUri, jsonSerializerOptions, cancellationToken);

            // We need to access the client to get the base address. Let's make sure it's there
            if (client == null) throw new ArgumentNullException(nameof(client));

            var query = HttpUtility.ParseQueryString(string.Empty);
            foreach (var item in queryParameters)
            {
                query[item.Key] = item.Value;
            }

            // If the client has a base address, use it.. else use the requestUri
            UriBuilder builder = client.BaseAddress != null ? new(client.BaseAddress + requestUri) : new(requestUri ?? "");

            // the requestUri might already have query parameters in it, join them then remove any tailing '&'
            builder.Query = string.Join('&', builder.Query, query.ToString()).Trim('&');

            // replace the requestUri with the new one, containing query parameters.
            // if the host is empty, we only want the PathAndQuery
            requestUri = string.IsNullOrWhiteSpace(builder.Host) ? builder.ToString() : builder.Uri.PathAndQuery;


            return client.GetFromJsonAsync<TValue>(requestUri, jsonSerializerOptions, cancellationToken);
        }

        public static Task<HttpResponseMessage> PostAsJsonAsync<TValue>(this HttpClient client, string? requestUri, IDictionary<string, string> queryParameters, TValue value, JsonSerializerOptions? jsonSerializerOptions = null, CancellationToken cancellationToken = default)
        {
            // If there are no query parameters then call the vanilla GetFromJsonAsync.
            if (!queryParameters.Any()) return client.PostAsJsonAsync(requestUri, value, jsonSerializerOptions, cancellationToken);

            // We need to access the client to get the base address. Let's make sure it's there
            if (client == null) throw new ArgumentNullException(nameof(client));

            var query = HttpUtility.ParseQueryString(string.Empty);
            foreach (var item in queryParameters)
            {
                query[item.Key] = item.Value;
            }

            // If the client has a base address, use it.. else use the requestUri
            UriBuilder builder = client.BaseAddress != null ? new(client.BaseAddress + requestUri) : new(requestUri ?? "");

            // the requestUri might already have query parameters in it, join them then remove any tailing '&'
            builder.Query = string.Join('&', builder.Query, query.ToString()).Trim('&');

            // replace the requestUri with the new one, containing query parameters.
            // if the host is empty, we only want the PathAndQuery
            requestUri = string.IsNullOrWhiteSpace(builder.Host) ? builder.ToString() : builder.Uri.PathAndQuery;

            return client.PostAsJsonAsync(requestUri, value, jsonSerializerOptions, cancellationToken);
        }

    }
}
