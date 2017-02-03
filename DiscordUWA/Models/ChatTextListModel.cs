using System;
using Windows.UI;

namespace DiscordUWA.Models {
    public class ChatTextListModel {
        public string Username { get; private set;}
        public Color UserRoleColor { get; private set; }
        public string ChatText { get; private set; }
        public string TimeSent { get; private set; }
        public string ImageUrl { get; private set; }

        public ChatTextListModel(string username, Color roleColor, string chatText, string timeSent, string imageUrl) {
            this.Username = username;
            this.UserRoleColor = roleColor;
            this.ChatText = chatText;
            this.TimeSent = timeSent;
            this.ImageUrl = imageUrl;
        }
    }
}
