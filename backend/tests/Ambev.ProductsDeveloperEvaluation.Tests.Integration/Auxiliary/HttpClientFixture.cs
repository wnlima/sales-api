using System.Net.Http.Headers;
using System.Net.Http.Json;

using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.ProductsDeveloperEvaluation.Infrastructure;
using Ambev.ProductsDeveloperEvaluation.WebApi;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

namespace Ambev.ProductsDeveloperEvaluation.Integration.Auxiliary
{
    public sealed class HttpClientFixture : IDisposable
    {
        internal const string COLLECTION_NAME = "HttpClient collection";
        private readonly CustomWebApplicationFactory<Program> _factory;
        private HttpClient? _client;
        public readonly DefaultContext DbContext;
        public readonly IServiceScope _scope;

        public HttpClientFixture()
        {
            _factory = new CustomWebApplicationFactory<Program>();
            _scope = Services.CreateScope();
            DbContext = _scope.ServiceProvider.GetRequiredService<DefaultContext>();
        }

        HttpClient Client
        {
            get
            {
                _client ??= _factory.CreateClient();
                return _client;
            }
        }

        public IServiceProvider Services => _factory.Services;

        public IConfigurationRoot Configuration => _factory.Configuration;

        public void SetDIBehaviors(params Action<IServiceCollection>[] behaviors)
        {
            _factory.SetDIBehaviors(behaviors);
        }

        public async Task<ApiResponseWithData<T>> DeserializeApiResponseWithData<T>(HttpResponseMessage response)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var ret = JsonConvert.DeserializeObject<ApiResponseWithData<T>>(responseContent);
            return ret;
        }

        public async Task<HttpResponseMessage> RequestSend(HttpMethod httpMethod, string requestUri, string token = "")
        {
            var request = new HttpRequestMessage(httpMethod, requestUri);

            if (!string.IsNullOrEmpty(token))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return await Client.SendAsync(request);
        }

        public async Task<HttpResponseMessage> RequestSend<TValue>(HttpMethod httpMethod, string requestUri, TValue value = null, string token = "") where TValue : class
        {
            var request = new HttpRequestMessage(httpMethod, requestUri);

            if (!string.IsNullOrEmpty(token))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            if (value != null)
            {
                JsonContent content = JsonContent.Create(value, mediaType: null);
                request.Content = content;
            }

            return await Client.SendAsync(request);
        }

        public void Dispose()
        {
            _scope.Dispose();
            _client?.Dispose();
            _factory?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
