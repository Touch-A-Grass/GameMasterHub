using GameMasterHub.Domain.Models;
using GameMasterHub.Infrastructure.Dto.Requests;
using GameMasterHub.Infrastructure.Dto.Responses;
using GameMasterHub.Infrastructure.Storage;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GameMasterHub.Infrastructure.Repositories
{
    public class TemplateRepository(HttpClient httpClient)
    {
        private readonly HttpClient _httpClient = httpClient;

        async public Task<bool> CreateTemplateCharacterAsync(TemplateCharacterModel templateCharacterModel)
        {
            try
            {
                var request = new CreateCharacterRequest
                {
                    Name = templateCharacterModel.Name,
                    Attributes = templateCharacterModel.Attributes,
                    Skills = templateCharacterModel.Skills
                };
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/template/character/", content);


                if (response?.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Failed to CreateCharacterAsync.");
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
            var mockData = new List<TemplateCharacterModel>
            {
                new TemplateCharacterModel
                {
                    Id = 1,
                    Name = "Warrior",
                    Attributes = new List<string>
                    {
                        "Strength",
                        "Endurance"
                    },
                    Skills = new List<string>
                    {
                        "Swordsmanship",
                        "Shield Defense"
                    }
                },
                new TemplateCharacterModel
                {
                    Id = 2,
                    Name = "Mage",
                    Attributes = new List<string>
                    {
                        "Intelligence",
                        "Wisdom"
                    },
                    Skills = new List<string>
                    {
                        "Spellcasting",
                        "Alchemy"
                    }
                },
                new TemplateCharacterModel
                {
                    Id = 3,
                    Name = "Rogue",
                    Attributes = new List<string>
                    {
                        "Dexterity",
                        "Agility"
                    },
                    Skills = new List<string>
                    {
                        "Stealth",
                        "Lockpicking"
                    }
                }
            };

            return mockData;
        }

    }
}
