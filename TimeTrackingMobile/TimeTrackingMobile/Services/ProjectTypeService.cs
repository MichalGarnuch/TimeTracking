using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TimeTrackingMobile.Models;

namespace TimeTrackingMobile.Services
{
    public class ProjectTypeService
    {
        private const string BaseUrl = "http://192.168.157.225:5215/api/ProjectType";
        private readonly HttpClient _client = new HttpClient();

        public async Task<List<ProjectTypeModel>> GetAllProjectTypes()
        {
            var result = await _client.GetFromJsonAsync<List<ProjectTypeModel>>(BaseUrl);
            return result ?? new List<ProjectTypeModel>();
        }

        public async Task<ProjectTypeModel> GetProjectType(int id)
        {
            var response = await _client.GetAsync($"{BaseUrl}/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ProjectTypeModel>();
        }

        public async Task<bool> CreateProjectType(ProjectTypeModel pt)
        {
            var response = await _client.PostAsJsonAsync(BaseUrl, pt);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateProjectType(int id, ProjectTypeModel pt)
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
