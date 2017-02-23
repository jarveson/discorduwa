using Windows.UI;

namespace DiscordUWA.Models {
    public class UserListModel {
        private string _avatarUrl = string.Empty;
        public string AvatarUrl {
            set {
                _avatarUrl = value ?? "https://discordapp.com/assets/322c936a8c8be1b803cd94861bdfa868.png";
            }
            get {
                return _avatarUrl;
            }
        }
        public Color UserRoleColor { get; set; } = Color.FromArgb(0xff, 0xff, 0xff, 0xff);
        public string CurrentlyPlaying { get; set; } = string.Empty;
        public Color StatusColor { get; set; } = Color.FromArgb(0xff, 0xff, 0xff, 0xff);
        public string Username { get; set; } = string.Empty;
        public string Nickname { get; set; } = string.Empty;
        public ulong Id { get; set; } = 0L;
        public bool IsBot { get; set; } = false;
    }
}
