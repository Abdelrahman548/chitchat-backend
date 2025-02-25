using ChitChat.Data.Entities.Abstracts;

namespace ChitChat.Service.DTOs.Response
{
    public class UserResponseDto : Entity
    {

        public string Name { get; set; } = string.Empty;
        public string About { get; set; } = string.Empty;
        public string PictureUrl { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public DateTime? LastSeen { get; set; }
        public bool LastSeenVisability { get; set; }
        public bool OnlineVisability { get; set; }

    }
}
