using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TimeTrackingMobile.Models;

namespace TimeTrackingMobile.Services
{
    public class ProjectTypeService
    {
        private const string BaseUrl = "http://localhost:5037/api/ProjectTypes";
        private readonly HttpClient _client = new HttpClient();

        public async Task<List<ProjectType>> GetAllProjectTypes()
        {
            var result = await _client.GetFromJsonAsync<List<ProjectType>>(BaseUrl);
            return result ?? new List<ProjectType>();
        }

        public async Task<ProjectType> GetProjectType(int id)
        {
            var response = await _client.GetAsync($"{BaseUrl}/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ProjectType>();
        }

        public async Task<bool> CreateProjectType(ProjectType pt)
        {
            var response = await _client.PostAsJsonAsync(BaseUrl, pt);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateProjectType(int id, ProjectType pt)
        {
            var response = await _client.PutAsJsonAsync($"{BaseUrl}/{id}", pt);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteProjectType(int id)
        {
            var response = await _client.DeleteAsync($"{BaseUrl}/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
