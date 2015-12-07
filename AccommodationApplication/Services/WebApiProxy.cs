using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using AccommodationApplication.Properties;

namespace AccommodationApplication.Services
{
    public abstract class WebApiProxy
    {
        private readonly HttpClient _client;
        private readonly MediaTypeFormatter _formatter;

        protected WebApiProxy(string controllerName, bool enableSsl=false)
        {
            _client = HttpClientFactory.Create();
            _formatter = new JsonMediaTypeFormatter();
            _client.BaseAddress =
                (!enableSsl)?new Uri($"{Settings.Default.BaseUrlAddress}/{controllerName}/"):
                new Uri($"{Settings.Default.SslUrlAddress}/{controllerName}/");
        }

        protected Task<TResponse> Post<TRequest, TResponse>(TRequest request)
        {
            return Post<TRequest, TResponse>(string.Empty, request);
        }

        protected async Task<TResponse> Post<TRequest, TResponse>(string method, TRequest request)
        {
            return await ExtractResponse<TResponse>(await _client.PostAsync(method, request, _formatter));
        }

        protected async Task<TResponse> Get<TResponse>(string uri)
        {
            return await ExtractResponse<TResponse>(await _client.GetAsync(uri));
        }

        private async Task<TResponse> ExtractResponse<TResponse>(HttpResponseMessage response)
        {
            await VerifyStatusCode(response);
            return await response.Content.ReadAsAsync<TResponse>(new[] { _formatter });
        }


        private async Task VerifyStatusCode(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                //TODO: Dedicated exception type
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }
    }
}
