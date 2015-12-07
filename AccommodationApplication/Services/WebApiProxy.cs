using AccommodationApplication.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationApplication.Services
{
    public abstract class WebApiProxy
    {
        private readonly HttpClient _httpClient;
        private readonly MediaTypeFormatter _formatter;

        protected WebApiProxy(string controllerName)
        {
            _httpClient = HttpClientFactory.Create();
            _formatter = new JsonMediaTypeFormatter();
            _httpClient.BaseAddress =
                new Uri(string.Format("{0}/{1}/", Settings.Default.BaseServicesUrl, controllerName));
           
        }

        protected Task<TResponse> Post<TRequest, TResponse>(TRequest request)
        {
            return Post<TRequest, TResponse>(string.Empty, request);
        }

        protected async Task<TResponse> Post<TRequest, TResponse>(string method, TRequest request)
        {
            return await ExtractResponse<TResponse>(await _httpClient.PostAsync(method, request, _formatter));
        }

        protected async Task<TResponse> Get<TResponse>(string uri)
        {
            return await ExtractResponse<TResponse>(await _httpClient.GetAsync(uri));
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
