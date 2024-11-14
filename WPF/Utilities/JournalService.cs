using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WPF.DTO.Journal;

namespace WPF.Utilities
{
    public class JournalService
    {
        private readonly HttpClient _client;

        public JournalService(string baseAddress)
        {
            _client = new HttpClient { BaseAddress = new Uri(baseAddress) };
        }

        public async Task<List<JournalDTO>> GetAllJournalAsync()
        {
            var response = await _client.GetAsync("API/Journal");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<List<JournalDTO>>();
        }

        public async Task<JournalDTO> GetJournalByIdAsync(int id)
        {
            var response = await _client.GetAsync($"API/Journal/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<JournalDTO>();
        }

        public async Task<JournalDTO> CreateJournalAsync(CreateJournalReqDTO journalModel)
        {
            var response = await _client.PostAsJsonAsync("API/Journal", journalModel);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<JournalDTO>();
        }

        public async Task<JournalDTO> UpdateJournalAsync(int id, UpdateJournalReqDTO journalDTO)
        {
            var response = await _client.PutAsJsonAsync($"API/Journal/{id}", journalDTO);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<JournalDTO>();
        }

        public async Task<bool> DeleteJournalAsync(int id)
        {
            var response = await _client.DeleteAsync($"API/Journal/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}