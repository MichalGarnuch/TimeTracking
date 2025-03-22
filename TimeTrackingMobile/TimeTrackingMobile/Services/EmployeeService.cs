using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TimeTrackingMobile.Models;

namespace TimeTrackingMobile.Services
{
    public class EmployeeService
    {
        private const string BaseUrl = "http://localhost:5037/api/Employees";
        private readonly HttpClient _client = new HttpClient();

        public async Task<List<Employee>> GetAllEmployees()
        {
            var result = await _client.GetFromJsonAsync<List<Employee>>(BaseUrl);
            return result ?? new List<Employee>();
        }

        public async Task<Employee> GetEmployee(int id)
        {
            var response = await _client.GetAsync($"{BaseUrl}/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Employee>();
        }

        public async Task<bool> CreateEmployee(Employee emp)
        {
            var response = await _client.PostAsJsonAsync(BaseUrl, emp);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateEmployee(int id, Employee emp)
        {
            var response = await _client.PutAsJsonAsync($"{BaseUrl}/{id}", emp);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteEmployee(int id)
        {
            var response = await _client.DeleteAsync($"{BaseUrl}/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
