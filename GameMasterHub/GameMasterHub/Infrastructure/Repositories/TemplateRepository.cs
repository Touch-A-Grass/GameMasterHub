using GameMasterHub.Infrastructure.Storage;
using System.Net.Http;

namespace GameMasterHub.Infrastructure.Repositories
{
    public class TemplateRepository(HttpClient httpClient)
    {
        private readonly HttpClient _httpClient = httpClient;
    }
}