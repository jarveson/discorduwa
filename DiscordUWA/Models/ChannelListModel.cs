﻿namespace DiscordUWA.Models {
    public class ChannelListModel {
        public ulong ChannelId { get; private set; }
        public string ChannelName { get; private set; }

        public ChannelListModel() {
            ChannelId = 0;
            ChannelName = "";
        }

        public ChannelListModel(ulong channelId, string channelName) {
            this.ChannelId = channelId;
            this.ChannelName = channelName;
        }
    }
}
