using Windows.UI;

namespace DiscordUWA.Models {
    public class UserListModel {
        public string AvatarUrl { get; set; }
        public Color UserRoleColor { get; set; } = Color.FromArgb(0xff, 0xff, 0xff, 0xff);
        public string CurrentlyPlaying { get; set; } = string.Empty;
        public Color StatusColor { get; set; } = Color.FromArgb(0xff, 0xff, 0xff, 0xff);
        public string Username { get; set; } = string.Empty;
        public string Nickname { get; set; } = string.Empty;
        public ulong Id { get; set; } = 0L;
        public bool IsBot { get; set; } = false;
    }
}
