using GameMasterHub.Infrastructure.Dto.Requests;
using GameMasterHub.Infrastructure.Dto.Responses;
using GameMasterHub.Infrastructure.Storage;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GameMasterHub.Infrastructure.Repositories
{
    public class AuthRepository(HttpClient httpClient, TokenStorage tokenStorage)
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly TokenStorage _tokenStorage = tokenStorage;

        async public Task LoginAsync(string username, string password)
        {
            
            _tokenStorage.Set("123");
            await Task.Delay(2000);
            _tokenStorage.Set(null);
            return;
            try
            {
                var request = new AuthRequest
                {
                    Username = username,
                    Password = password
                };
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/auth/login", content);
                var authResponse = JsonSerializer.Deserialize<AuthResponse>(await response.Content.ReadAsStringAsync());

                _tokenStorage.Set(authResponse?.Token);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        
        async public Task RegisterAsync(string username, string password)
        {
            _tokenStorage.Set("123");
            await Task.Delay(2000);
            _tokenStorage.Set(null);
            return;
            try
            {
                var request = new AuthRequest
                {
                    Username = username,
                    Password = password
                };
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/auth/register", content);
                var authResponse = JsonSerializer.Deserialize<AuthResponse>(await response.Content.ReadAsStringAsync());

                _tokenStorage.Set(authResponse?.Token);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public IObservable<string?> WatchToken()
        {
            return _tokenStorage.Watch();
        }

        
    }
}
