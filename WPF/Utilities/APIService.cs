using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

namespace WPF.Utilities
{
    public class APIService
    {
        private static readonly HttpClient _client = new HttpClient();

        public APIService()
        {
            _client.BaseAddress = new Uri("");
            _client.DefaultRequestHeaders.Accept.Clear();
        }
    }
}