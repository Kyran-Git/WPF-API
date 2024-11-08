using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace WPF.Utilities
{
    public class APIService
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public APIService()
        {
            _httpClient.BaseAddress = new Uri("http://localhost:5053");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
        }

        public async Task<T> GetAsync<T>(string endpoint)
    {
        HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
        
        if (response.IsSuccessStatusCode)
        {
            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        throw new HttpRequestException("Request failed with status: " + response.StatusCode);
    }
    }
}