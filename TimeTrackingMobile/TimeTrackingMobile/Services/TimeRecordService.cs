using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TimeTrackingMobile.Models;

namespace TimeTrackingMobile.Services
{
    public class TimeRecordService
    {
        private const string BaseUrl = "http://192.168.145.225:5215/api/TimeRecord";
        private readonly HttpClient _client = new HttpClient();

        public async Task<List<TimeRecordModel>> GetAllTimeRecords()
        {
            var result = await _client.GetFromJsonAsync<List<TimeRecordModel>>(BaseUrl);
            return result ?? new List<TimeRecordModel>();
        }

        public async Task<TimeRecordModel> GetTimeRecord(int id)
        {
            var response = await _client.GetAsync($"{BaseUrl}/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TimeRecordModel>();
        }

        public async Task<bool> CreateTimeRecord(TimeRecordModel record)
        {
            var response = await _client.PostAsJsonAsync(BaseUrl, record);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateTimeRecord(int id, TimeRecordModel record)
        {
            var response = await _client.PutAsJsonAsync($"{BaseUrl}/{id}", record);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteTimeRecord(int id)
        {
            var response = await _client.DeleteAsync($"{BaseUrl}/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
