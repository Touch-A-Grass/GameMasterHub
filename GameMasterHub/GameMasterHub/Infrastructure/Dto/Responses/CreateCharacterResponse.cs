using System.Collections.Generic;

namespace GameMasterHub.Infrastructure.Dto.Responses
{
    public class CreateCharacterResponse
    {
        public int GameId { get; set; }
        public List<AttributeResponseDto> Attributes { get; set; }
        public List<SkillResponseDto> Skills { get; set; }
    }

    public class AttributeResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }

    public class SkillResponseDto
    {
        public string Name { get; set; }
        public string Attribute { get; set; }
    }
        
}
