using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using WPF.DTO.Entry;

namespace WPF.Utilities
{
    public class EntryService
    {
        private readonly HttpClient _client;

        public EntryService(string baseAdrress)
        {
            _client = new HttpClient { BaseAddress = new Uri(baseAdrress) };
        }

        public async Task<List<EntryDTO>> GetAllEntryAsync()
        {
            var response = await _client.GetAsync("API/Entry");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<List<EntryDTO>>();
        }

        public async Task<EntryDTO> GetEntryByIdAsync(int id)
        {
            var response = await _client.GetAsync($"API/Entry/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<EntryDTO>();
        }

        public async Task<EntryDTO> CreateEntryAsync(int journalId, CreateEntryDTO entryDTO)
        {
            var response = await _client.PostAsJsonAsync($"API/Entry/{journalId}", entryDTO );
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<EntryDTO>();
        }

        public async Task<EntryDTO> UpdateEntryAsync(int id, UpdateEntryReqDTO entryDTO)
        {
            var response = await _client.PutAsJsonAsync($"API/Entry/{id}", entryDTO);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<EntryDTO>();
        }

        public async Task<EntryDTO> DeleteEntryAsync(int id)
        {
            var response = await _client.DeleteAsync($"API/Entry/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<EntryDTO>();
        }
    }
}