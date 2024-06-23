using GameMasterHub.Domain.Models;
using GameMasterHub.Infrastructure.Dto.Requests;
using GameMasterHub.Infrastructure.Dto.Responses;
using GameMasterHub.Infrastructure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GameMasterHub.Infrastructure.Repositories
{
    public class GameRepository(HttpClient httpClient, GameStorage gameStorage)
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly GameStorage _gameStorage = gameStorage;

        async public Task<bool> CreateTemplateCharacterAsync(TemplateCharacterModel templateCharacterModel)
        {
            try
            {
                var request = new CreateCharacterRequest
                {
                    GameId = templateCharacterModel.GameId,
                    AttributesWallet = templateCharacterModel.AttributesWallet,
                    Attributes = templateCharacterModel.Attributes.Select(a => new AttributeDto
                    {
                        Name = a.Name,
                        Type = a.Type
                    }).ToList(),
                    Skills = templateCharacterModel.Skills.Select(s => new SkillDto
                    {
                        Name = s.Name,
                        Attribute = s.Attribute.Name
                    }).ToList()
                };
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                Console.WriteLine(content.ToString());

                var responseJson = @"
{
    ""game_id"": 1,
    ""attributes"": [
        { ""id"": 1, ""name"": ""strength"", ""type"": ""int"" }, 
        { ""id"": 2, ""name"": ""intellect"", ""type"": ""int"" }, 
        { ""id"": 3, ""name"": ""cursed"", ""type"": ""bool"" }
    ],
    ""skills"": [
        { ""name"": ""harizma"", ""attribute"": ""intellect"" }, 
        { ""name"": ""sex"", ""attribute"": ""strength"" }
    ]
}";

                var response = JsonSerializer.Deserialize<CreateCharacterResponse>(responseJson);

                var templateCharacter = new TemplateCharacterModel
                {
                    GameId = response.GameId,
                    Attributes = response.Attributes.Select(a => new AttributeModel
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Type = a.Type
                    }).ToList(),
                    Skills = response.Skills.Select(s => new SkillModel
                    {
                        Name = s.Name,
                        Attribute = response.Attributes
                            .Where(a => a.Name == s.Attribute)
                            .Select(a => new AttributeModel
                            {
                                Id = a.Id,
                                Name = a.Name,
                                Type = a.Type
                            })
                            .FirstOrDefault()
                    }).ToList()
                };

                _gameStorage.Set(templateCharacter);

                return true;

                /*var response = await _httpClient.PostAsync("/game/stats/", content);


                if (response?.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Failed to CreateCharacterAsync.");
                    return false;
                }*/
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return false;
        }

        public async Task<bool> CreateGame(string gameName)
        {
            await Task.Delay(2000);
            return true;
            try
            {
                var registerRequest = new CreateGameRequest
                {
                    Name = gameName
                };
                var json = JsonSerializer.Serialize(registerRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/game/create/", content);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var createGameResponse = JsonSerializer.Deserialize<CreateGameResponse>(responseBody);
                    if (createGameResponse != null)
                    {
                        _gameStorage.GameId = createGameResponse.Id;
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Failed to deserialize the response.");
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine($"Failed to create game. Status code: {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return false;
        }

        public async Task<List<TemplateCharacterModel>> GetTemplatesCharactersAsync()
        {
            throw new NotImplementedException();
        }

        public int GetGameId()
        {
            return _gameStorage.GameId;
        }

        public IObservable<TemplateCharacterModel> WatchGame()
        {
            return _gameStorage.Watch();
        }

        public void ClearStorage()
        {
            _gameStorage.Clear();
        }
    }
}
