using System;

namespace DiscordUWA.Models {
    public class ServerListModel {
        public ulong ServerId { get; private set; }
        public string ServerName { get; private set; }
        public string ServerImageUri { get; private set; }

        public ServerListModel(ulong serverId, string serverName, string serverUri) {
            this.ServerName = serverName;
            this.ServerImageUri = serverUri == null ? "x" : serverUri;
            this.ServerId = serverId;
        }
    }
}
