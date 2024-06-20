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
            try
            {
                var request = new AuthRequest
                {
                    Login = username,
                    Password = password
                };
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/user/login/", content);

                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Login Response Body: " + responseBody);

                var authResponse = JsonSerializer.Deserialize<AuthResponse>(responseBody);
                Console.WriteLine($"Set token {authResponse?.Token}");

                if (authResponse?.Token != null)
                {
                    _tokenStorage.Set(authResponse.Token);
                }
                else
                {
                    Console.WriteLine("Failed to deserialize the authentication response.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public async Task RegisterAsync(string username, string password)
        {
            try
            {
                var registerRequest = new RegisterRequest
                {
                    Login = username,
                    Username = username,
                    Password = password
                };
                var json = JsonSerializer.Serialize(registerRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var registerResponse = await _httpClient.PostAsync("/user", content);
                var registerResponseBody = await registerResponse.Content.ReadAsStringAsync();

                if (!registerResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Registration failed with status code: {registerResponse.StatusCode}");
                    return;
                }

                var loginRequest = new AuthRequest
                {
                    Login = username,
                    Password = password
                };
                json = JsonSerializer.Serialize(loginRequest);
                content = new StringContent(json, Encoding.UTF8, "application/json");

                var loginResponse = await _httpClient.PostAsync("/user/login", content);

                var loginResponseBody = await loginResponse.Content.ReadAsStringAsync();
                var authResponse = JsonSerializer.Deserialize<AuthResponse>(loginResponseBody);

                if (authResponse != null)
                {
                    _tokenStorage.Set(authResponse.Token);
                }
                else
                {
                    Console.WriteLine("Failed to deserialize the authentication response.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public IObservable<string?> WatchToken()
        {
            return _tokenStorage.Watch();
        }

        public void Logout()
        {
            _tokenStorage.Clear();
        }
    }
}
