using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Configuration;
using TRMDesktopUI.Models;

namespace TRMDesktopUI.Helpers
{
    public class APIHelper : IAPIHelper
    {
        private HttpClient _apiClient { get; set; }

        public APIHelper()
        {
            InitializeClient();
        }

        private void InitializeClient()
        {
            var api = ConfigurationManager.AppSettings["api"];

            _apiClient = new HttpClient();
            _apiClient.BaseAddress = new Uri(api);
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<AuthenticatedUser> Authenticate(string username, string password)
        {
            var data = new FormUrlEncodedContent(new[]{
                new KeyValuePair<string,string>("grant_type", "password"),
                new KeyValuePair<string,string>("username", username),
                new KeyValuePair<string,string>("password", password)
                });

            using (HttpResponseMessage response = await _apiClient.PostAsync("/Token", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<AuthenticatedUser>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
