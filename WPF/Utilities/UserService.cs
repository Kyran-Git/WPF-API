using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WPF.DTO.Users;
using System.Net.Http.Json;

namespace WPF.Utilities
{
    public class UserService
    {
        private readonly HttpClient _client;

        public UserService(string baseAddress)
        {
            _client = new HttpClient { BaseAddress = new Uri(baseAddress) };
        }

        public async Task<List<UserDTO>> GetUsersAsync()
        {
            var response = await _client.GetAsync("API/Users");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<List<UserDTO>>();
        }

        public async Task<UserDTO> GetUserById(int id)
        {
            var response = await _client.GetAsync($"API/Users/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<UserDTO>();
        }

        public async Task<UserDTO> CreateUserAsync(CreateUserDTO userModel)
        {
            var response = await _client.PostAsJsonAsync($"API/Users", userModel);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<UserDTO>();
        }

        public async Task<UserDTO> UpdateUserAsync(int id, UpdateUserDTO userDTO)
        {
            var response = await _client.PutAsJsonAsync($"API/Users/{id}", userDTO);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<UserDTO>();
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var response = await _client.DeleteAsync($"API/Users/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<UserDTO> AuthenticateAsync(string username, string password)
        {
            try
            {
                var response = await _client.PostAsJsonAsync("api/auth/login", new
                {
                    Username = username,
                    Password = password
                });

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<UserDTO>();
            }
            catch
            {
                return null;
            }
        }
    }
}
