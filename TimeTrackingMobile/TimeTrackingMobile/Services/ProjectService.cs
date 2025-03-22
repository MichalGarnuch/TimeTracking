using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TimeTrackingMobile.Models;

namespace TimeTrackingMobile.Services
{
    public class ProjectService
    {
        private const string BaseUrl = "http://localhost:5037/api/Projects";
        private readonly HttpClient _client = new HttpClient();

        public async Task<List<Project>> GetAllProjects()
        {
            var result = await _client.GetFromJsonAsync<List<Project>>(BaseUrl);
            return result ?? new List<Project>();
        }

        public async Task<Project> GetProject(int id)
        {
            var response = await _client.GetAsync($"{BaseUrl}/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Project>();
        }

        public async Task<bool> CreateProject(Project project)
        {
            var response = await _client.PostAsJsonAsync(BaseUrl, project);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateProject(int id, Project project)
        {
            var response = await _client.PutAsJsonAsync($"{BaseUrl}/{id}", project);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteProject(int id)
        {
            var response = await _client.DeleteAsync($"{BaseUrl}/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
