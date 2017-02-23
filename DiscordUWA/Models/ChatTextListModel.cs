using System;
using System.Collections.Generic;
using Windows.UI;

namespace DiscordUWA.Models {
    public class ChatTextListModel {
        public ulong Id { get; set; }
        public string Username { get; set; }
        public Color UserRoleColor { get; set; }
        public string ChatText { get; set; }
        public string TimeSent { get; set; }
        public string AvatarUrl { get; set; }
        public string TimeEdited { get; set; } = string.Empty;
        public IEnumerable<Discord.IEmbed> Embeds { get; set; }
        public IEnumerable<Discord.IAttachment> Attachments { get; set; }
    }
}
