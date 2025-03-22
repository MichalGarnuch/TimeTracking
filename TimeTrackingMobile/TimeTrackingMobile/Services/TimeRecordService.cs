using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TimeTrackingMobile.Models;

namespace TimeTrackingMobile.Services
{
    public class TimeRecordService
    {
        private const string BaseUrl = "http://192.168.114.225:5215/api/TimeRecord";
        private readonly HttpClient _client = new HttpClient();

        public async Task<List<TimeRecord>> GetAllTimeRecords()
        {
            var result = await _client.GetFromJsonAsync<List<TimeRecord>>(BaseUrl);
            return result ?? new List<TimeRecord>();
        }

        public async Task<TimeRecord> GetTimeRecord(int id)
        {
            var response = await _client.GetAsync($"{BaseUrl}/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TimeRecord>();
        }

        public async Task<bool> CreateTimeRecord(TimeRecord record)
        {
            var response = await _client.PostAsJsonAsync(BaseUrl, record);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateTimeRecord(int id, TimeRecord record)
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
