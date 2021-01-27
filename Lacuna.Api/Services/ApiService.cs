using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Lacuna.Api.Utils;
using Lacuna.Domain.Entities;
using Lacuna.Domain.Services;
using Microsoft.Extensions.Configuration;

namespace Lacuna.Api.Services
{
    public class ApiService : IApiService
    {
        private HttpClient _httpClient;
        private IConfigurationSection _settings;

        private async Task<Dictionary<string, string>> PostAsync(string endpoint, object data = null)
        {
            var response = await _httpClient.PostAsync(endpoint, data!=null? HttpJsonUtils.GenerateContent(data) : null);

            if(response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var content = HttpJsonUtils.DeserializeToDict(json);

                if(content["status"]=="Success")
                    return content;

                return null;
            }

            return null;
        }

        private async Task<Dictionary<string, string>> GetAsync(string endpoint, string token)
        {
            if(!string.IsNullOrEmpty(token))
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var response = await _httpClient.GetAsync(endpoint);

            if(response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var content = HttpJsonUtils.DeserializeToDict(json);

                if(content["status"]=="Success")
                    return content;

                return null;
            }

            return null;
        }

        public ApiService(IConfigurationSection settings, IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("ApiHttpClient");
            _settings = settings;
        }

        public bool CreateUser(User user)
        {
            var result = PostAsync(_settings.GetValue<string>("CreateUser"), new {
                username=user.Name,
                password=user.Password,
                email=_settings.GetValue<string>("UserEmail")
            }).Result;

            return result!=null;
        }

        public Token Login(User user)
        {
            var result = PostAsync(_settings.GetValue<string>("Login"), new {
                username=user.Name,
                password=user.Password,
            }).Result;

            return result==null? null : new Token(result["token"]);
        }

        public string GetSecret(Token token)
        {
            var result = GetAsync(_settings.GetValue<string>("Secret"), token.ToString()).Result;

            if(result==null)
                return null;

            else
                return result["secret"];
        }
    }
}