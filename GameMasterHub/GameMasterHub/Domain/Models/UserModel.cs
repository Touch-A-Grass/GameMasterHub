using System.ComponentModel.DataAnnotations;

namespace GameMasterHub.Domain.Models
{
    public class UserModel
    {
        public string Username { get; set; }
        public string? Avatar { get; set; }
    }
}
