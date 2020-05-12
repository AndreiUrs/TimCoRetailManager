using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Configuration;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.Library.Api
{
    public class APIHelper : IAPIHelper
    {
        private HttpClient apiClient { get; set; }
        private ILoggedInUserModel _loggedInUserModel;

        public APIHelper(ILoggedInUserModel loggedInUserModel)
        {
            _loggedInUserModel = loggedInUserModel;
            InitializeClient();
        }

        private void InitializeClient()
        {
            var api = ConfigurationManager.AppSettings["api"];

            apiClient = new HttpClient();
            apiClient.BaseAddress = new Uri(api);
            apiClient.DefaultRequestHeaders.Accept.Clear();
            apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<AuthenticatedUser> Authenticate(string username, string password)
        {
            var data = new FormUrlEncodedContent(new[]{
                new KeyValuePair<string,string>("grant_type", "password"),
                new KeyValuePair<string,string>("username", username),
                new KeyValuePair<string,string>("password", password)
                });

            using (HttpResponseMessage response = await apiClient.PostAsync("/Token", data))
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

        public async Task GetLoggedInUserInfo(string token)
        {
            apiClient.DefaultRequestHeaders.Clear();
            apiClient.DefaultRequestHeaders.Accept.Clear();
            apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            using (HttpResponseMessage response = await apiClient.GetAsync("/api/User"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var results = await response.Content.ReadAsAsync<LoggedInUserModel>();

                    _loggedInUserModel.CreatedDate = results.CreatedDate;
                    _loggedInUserModel.EmailAddress = results.EmailAddress;
                    _loggedInUserModel.FirstName = results.FirstName;
                    _loggedInUserModel.Id = results.Id;
                    _loggedInUserModel.LastName = results.LastName;
                    _loggedInUserModel.Token = token;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
