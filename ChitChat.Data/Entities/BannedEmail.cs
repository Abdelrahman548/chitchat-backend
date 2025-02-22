using ChitChat.Data.Entities.Abstracts;
using System.ComponentModel.DataAnnotations;

namespace ChitChat.Data.Entities
{
    public class BannedEmail : Entity
    {
        [EmailAddress]
        [MaxLength(128)]
        public string Email { get; set; } = string.Empty;
    }
}
