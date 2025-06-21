using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TimeTrackingMobile.Models;

namespace TimeTrackingMobile.Services
{
    public class ProjectService
    {
        private const string BaseUrl = "http://192.168.237.225:5215/api/Project";
        private readonly HttpClient _client = new HttpClient();

        public async Task<List<ProjectModel>> GetAllProjects()
        {
            var result = await _client.GetFromJsonAsync<List<ProjectModel>>(BaseUrl);
            return result ?? new List<ProjectModel>();
        }

        public async Task<ProjectModel> GetProject(int id)
        {
            var response = await _client.GetAsync($"{BaseUrl}/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ProjectModel>();
        }

        public async Task<bool> CreateProject(ProjectModel project)
        {
            var response = await _client.PostAsJsonAsync(BaseUrl, project);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateProject(int id, ProjectModel project)
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
