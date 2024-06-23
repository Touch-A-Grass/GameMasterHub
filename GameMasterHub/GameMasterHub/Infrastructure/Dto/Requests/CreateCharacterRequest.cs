using System.Collections.Generic;

namespace GameMasterHub.Infrastructure.Dto.Requests
{
    public class CreateCharacterRequest
    {
        public int GameId { get; set; }
        public int AttributesWallet { get; set; }
        public List<AttributeDto> Attributes { get; set; }
        public List<SkillDto> Skills { get; set; }
    }

    public class AttributeDto
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }

    public class SkillDto
    {
        public string Name { get; set; }
        public string Attribute { get; set; }
    }
}
