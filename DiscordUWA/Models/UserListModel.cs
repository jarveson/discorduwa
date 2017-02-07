using Windows.UI;

namespace DiscordUWA.Models {
    public class UserListModel {
        public string AvatarUrl { get; private set; }
        public Color UserRoleColor { get; private set; }
        public string CurrentlyPlaying { get; private set; }
        public Color StatusColor { get; private set; }
        public string Username { get; private set; }
        public ulong Id {get; private set;}
        public bool IsBot {get; private set;}

        public UserListModel() {
            this.AvatarUrl = "https://discordapp.com/assets/322c936a8c8be1b803cd94861bdfa868.png";
            this.CurrentlyPlaying = "";
            this.Username = "";
            this.StatusColor = Color.FromArgb(0xff, 0xff, 0xff, 0xff);
            this.UserRoleColor = Color.FromArgb(0xff, 0xff, 0xff, 0xff);
            this.Id = 0;
            this.IsBot = false;
        }

        public UserListModel(string avatarUrl, string currentlyPlaying, Color statusColor, string username, Color userRoleColor, ulong id, bool isBot) {
            this.AvatarUrl = avatarUrl == null ? "https://discordapp.com/assets/322c936a8c8be1b803cd94861bdfa868.png" : avatarUrl;
            this.CurrentlyPlaying = currentlyPlaying;
            this.StatusColor = statusColor;
            this.Username = username;
            this.UserRoleColor = userRoleColor;
            this.Id = id;
            this.IsBot = isBot;
        }
    }
}
