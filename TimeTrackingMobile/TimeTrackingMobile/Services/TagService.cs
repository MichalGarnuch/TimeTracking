using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TimeTrackingMobile.Models;

namespace TimeTrackingMobile.Services
{
    public class TagService
    {
        private const string BaseUrl = "http://localhost:5037/api/Tag";
        private readonly HttpClient _client = new HttpClient();

        public async Task<List<Tag>> GetAllTags()
        {
            var result = await _client.GetFromJsonAsync<List<Tag>>(BaseUrl);
            return result ?? new List<Tag>();
        }

        public async Task<Tag> GetTag(int id)
        {
            var response = await _client.GetAsync($"{BaseUrl}/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Tag>();
        }

        public async Task<bool> CreateTag(Tag tag)
        {
            var response = await _client.PostAsJsonAsync(BaseUrl, tag);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateTag(int id, Tag tag)
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
