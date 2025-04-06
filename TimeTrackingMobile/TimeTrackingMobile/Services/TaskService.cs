using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TaskModel = TimeTrackingMobile.Models.TaskModel;

namespace TimeTrackingMobile.Services
{
    public class TaskService
    {
        private const string BaseUrl = "http://192.168.8.225:5215/api/Task";
        private readonly HttpClient _client = new HttpClient();

        public async Task<List<TaskModel>> GetAllTasks()
        {
            var result = await _client.GetFromJsonAsync<List<TaskModel>>(BaseUrl);
            return result ?? new List<TaskModel>();
        }

        public async Task<TaskModel> GetTask(int id)
        {
            var response = await _client.GetAsync($"{BaseUrl}/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TaskModel>();
        }

        public async Task<bool> CreateTask(TaskModel task)
        {
            var response = await _client.PostAsJsonAsync(BaseUrl, task);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateTask(int id, TaskModel task)
        {
            var response = await _client.PutAsJsonAsync($"{BaseUrl}/{id}", task);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteTask(int id)
        {
            var response = await _client.DeleteAsync($"{BaseUrl}/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
