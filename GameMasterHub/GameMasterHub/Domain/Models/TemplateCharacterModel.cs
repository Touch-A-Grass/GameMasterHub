using System.Collections.Generic;

namespace GameMasterHub.Domain.Models
{
    public class TemplateCharacterModel
    {
        public int? Id { get; set; }

        public int GameId { get; set; }
        public int AttributesWallet { get; set; }
        public List<AttributeModel> Attributes { get; set; } = new List<AttributeModel>();
        public List<SkillModel> Skills { get; set; } = new List<SkillModel>();
    }
}
