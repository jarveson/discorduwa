using System;

namespace DiscordUWA.Models {
    public class ServerListModel {
        public ulong ServerId { get; set; } = 0;
        public string ServerName { get; set; } = String.Empty;
        public string ServerImageUri { get; set; } = String.Empty;
    }
}
