using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TimeTrackingMobile.Models;

namespace TimeTrackingMobile.Services
{
    public class TagService
    {
        private const string BaseUrl = "http://192.168.145.225:5215/api/Tag";
        private readonly HttpClient _client = new HttpClient();

        public async Task<List<TagModel>> GetAllTags()
        {
            var result = await _client.GetFromJsonAsync<List<TagModel>>(BaseUrl);
            return result ?? new List<TagModel>();
        }

        public async Task<TagModel> GetTag(int id)
        {
            var response = await _client.GetAsync($"{BaseUrl}/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TagModel>();
        }

        public async Task<bool> CreateTag(TagModel tag)
        {
            var response = await _client.PostAsJsonAsync(BaseUrl, tag);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateTag(int id, TagModel tag)
        {
            var response = await _client.PutAsJsonAsync($"{BaseUrl}/{id}", tag);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteTag(int id)
        {
            var response = await _client.DeleteAsync($"{BaseUrl}/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
