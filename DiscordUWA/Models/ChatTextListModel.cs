using System;

namespace DiscordUWA.Models {
    //todo: this will probably want to be bindable base with all of these as the setproperty thing to handle
    //      edits n shit like that.
    public class ChatTextListModel {
        public string Username { get; private set;}
        public string UserRoleColor { get; private set; }
        public string ChatText { get; private set; }
        public string TimeSent { get; private set; }
        public string ImageUrl { get; private set; }

        public ChatTextListModel(string username, string roleColor, string chatText, string timeSent, string imageUrl) {
            this.Username = username;
            this.UserRoleColor = roleColor;
            this.ChatText = chatText;
            this.TimeSent = timeSent;
            this.ImageUrl = imageUrl;
        }
    }
}
