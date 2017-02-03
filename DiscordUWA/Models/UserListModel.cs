using Windows.UI;
using Windows.UI.Xaml.Media;

namespace DiscordUWA.Models {
    public class UserListModel {
        public string AvatarUrl { get; private set; }
        public string UserRoleColor { get; private set; }
        public string CurrentlyPlaying { get; private set; }
        public bool IsIdle { get; private set; }
        public string Username { get; private set; }
        public ulong Id {get; private set;}

        public UserListModel(string avatarUrl, string currentlyPlaying, bool isIdle, string username, string userRoleColor, ulong id) {
            this.AvatarUrl = avatarUrl;//== null ? "": avatarUrl;
            this.CurrentlyPlaying = currentlyPlaying;
            this.IsIdle = isIdle;
            this.Username = IsIdle ? $"{username}(idle)" : username;
            this.UserRoleColor = userRoleColor;
            this.Id = id;
        }
    }
}
