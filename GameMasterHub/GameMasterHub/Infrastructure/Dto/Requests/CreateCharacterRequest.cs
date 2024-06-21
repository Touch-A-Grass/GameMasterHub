using System.Collections.Generic;

namespace GameMasterHub.Infrastructure.Dto.Requests
{
    public class CreateCharacterRequest
    {
        public string Name { get; set; }
        public List<string> Attributes { get; set; }
        public List<string> Skills { get; set; }
    }
}
