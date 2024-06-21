using System.Collections.Generic;

namespace GameMasterHub.Domain.Models
{
    public class TemplateCharacterModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Attributes { get; set; }
        public List<string> Skills { get; set; }
    }
}
