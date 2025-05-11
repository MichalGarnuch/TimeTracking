using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TimeTrackingMobile.Models;

namespace TimeTrackingMobile.Services
{
    public class DepartmentService
    {
        private const string BaseUrl = "http://192.168.130.225:5215/api/Department";
        private readonly HttpClient _client;

        public DepartmentService()
        {
            _client = new HttpClient();
        }

        public async Task<List<DepartmentModel>> GetAllDepartments()
        {
            try
            {
                var result = await _client.GetFromJsonAsync<List<DepartmentModel>>(BaseUrl);
                return result ?? new List<DepartmentModel>();
            }
            catch (HttpRequestException httpEx)
            {
                await App.Current.MainPage.DisplayAlert("Błąd HTTP", httpEx.Message, "OK");
                return new List<DepartmentModel>();
            }
            catch (System.Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Inny błąd", ex.Message, "OK");
                return new List<DepartmentModel>();
            }
        }


        public async Task<DepartmentModel> GetDepartment(int id)
        {
            var response = await _client.GetAsync($"{BaseUrl}/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<DepartmentModel>();
        }

        public async Task<bool> CreateDepartment(DepartmentModel dept)
        {
            var response = await _client.PostAsJsonAsync(BaseUrl, dept);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateDepartment(int id, DepartmentModel dept)
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
