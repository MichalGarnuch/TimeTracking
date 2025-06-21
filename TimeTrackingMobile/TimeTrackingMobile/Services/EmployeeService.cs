using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TimeTrackingMobile.Models;

namespace TimeTrackingMobile.Services
{
    public class EmployeeService
    {
        private const string BaseUrl = "http://192.168.237.225:5215/api/Employee";
        private readonly HttpClient _client = new HttpClient();

        public async Task<List<EmployeeModel>> GetAllEmployees()
        {
            var result = await _client.GetFromJsonAsync<List<EmployeeModel>>(BaseUrl);
            return result ?? new List<EmployeeModel>();
        }

        public async Task<EmployeeModel> GetEmployee(int id)
        {
            var response = await _client.GetAsync($"{BaseUrl}/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<EmployeeModel>();
        }

        public async Task<List<EmployeeModel>> GetEmployeesByDepartment(int departmentId)
        {
            var url = $"{BaseUrl}/by-department/{departmentId}";
            var result = await _client.GetFromJsonAsync<List<EmployeeModel>>(url);
            return result ?? new List<EmployeeModel>();
        }

        public async Task<bool> CreateEmployee(EmployeeModel emp)
        {
            var response = await _client.PostAsJsonAsync(BaseUrl, emp);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateEmployee(int id, EmployeeModel emp)
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
