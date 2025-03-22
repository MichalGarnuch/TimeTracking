using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TimeTrackingMobile.Models;

namespace TimeTrackingMobile.Services
{
    public class DepartmentService
    {
        private const string BaseUrl = "http://localhost:5037/api/Departments";
        private readonly HttpClient _client;

        public DepartmentService()
        {
            _client = new HttpClient();
        }

        public async Task<List<Department>> GetAllDepartments()
        {
            var result = await _client.GetFromJsonAsync<List<Department>>(BaseUrl);
            return result ?? new List<Department>();
        }

        public async Task<Department> GetDepartment(int id)
        {
            var response = await _client.GetAsync($"{BaseUrl}/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Department>();
        }

        public async Task<bool> CreateDepartment(Department dept)
        {
            var response = await _client.PostAsJsonAsync(BaseUrl, dept);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateDepartment(int id, Department dept)
        {
            var response = await _client.PutAsJsonAsync($"{BaseUrl}/{id}", dept);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteDepartment(int id)
        {
            var response = await _client.DeleteAsync($"{BaseUrl}/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
