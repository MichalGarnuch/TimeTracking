using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TimeTrackingMobile.Models;

namespace TimeTrackingMobile.Services
{
    public class TaskTagService
    {
        private const string BaseUrl = "http://192.168.237.225:5215/api/TaskTag";
        private readonly HttpClient _client = new HttpClient();

        public async Task<List<TaskTagModel>> GetAllTaskTags()
        {
            var result = await _client.GetFromJsonAsync<List<TaskTagModel>>(BaseUrl);
            return result ?? new List<TaskTagModel>();
        }

        public async Task<string> CreateTaskTag(TaskTagModel taskTag)
        {
            try
            {
                var response = await _client.PostAsJsonAsync(BaseUrl, taskTag);
                if (response.IsSuccessStatusCode)
                    return null;

                var message = await response.Content.ReadAsStringAsync();
                return string.IsNullOrWhiteSpace(message)
                    ? response.ReasonPhrase
                    : message;
            }
            catch (HttpRequestException ex)
            {
                return ex.Message;
            }
        }

        public async Task<bool> DeleteTaskTag(int taskId, int tagId)
        {
            var url = $"{BaseUrl}/{taskId}/{tagId}";
            var response = await _client.DeleteAsync(url);
            return response.IsSuccessStatusCode;
        }
    }
}
