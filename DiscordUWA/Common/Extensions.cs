using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace DiscordUWA.Common {
    public static class Extensions {
        public static Windows.UI.Color ToWinColor(this Discord.Color color) {
            if (color.RawValue == 0)
                return Windows.UI.Color.FromArgb(0x99, 0xff, 0xff, 0xff);
            return Windows.UI.Color.FromArgb(0xff, color.R, color.G, color.B);
        }

        public static Windows.UI.Color ToWinColor(this Discord.UserStatus status) {
            if (status == Discord.UserStatus.Online)
                return Windows.UI.Color.FromArgb(0xff, 0x43, 0xb5, 0x81);
            else if (status == Discord.UserStatus.Idle || status == Discord.UserStatus.AFK)
                return Windows.UI.Color.FromArgb(0xff, 0xfa, 0xa6, 0x1a);
            else if (status == Discord.UserStatus.DoNotDisturb)
                return Windows.UI.Color.FromArgb(0xff, 0xf0, 0x47, 0x47);
            else return Windows.UI.Color.FromArgb(0xff, 0x74, 0x7f, 0x8d);
        }

        public static string GetReplacedMessageText(this Discord.IMessage message) {
            string chatText = message.Content;
            // whats going to happen here is we change the discord tag to also include the text
            // todo: theres probably a better / faster way to do this?
            foreach (var user in message.MentionedUsers) {
                // nickname
                chatText = chatText.Replace($"<@!{user.Id}>", $"<@!{user.Id}:{user.Nickname}>");
                // regular name
                chatText = chatText.Replace($"<@{user.Id}>", $"<@{user.Id}:{user.Name}>");
            }
            foreach (var role in message.MentionedRoles) {
                chatText = chatText.Replace($"<@&{role.Id}>", $"<@&{role.Id}:{role.Name}>");
            }
            foreach(var channel in message.MentionedChannels) {
                chatText = chatText.Replace($"<@#{channel.Id}>", $"<@#{channel.Id}:{channel.Name}>");
            }

            return chatText;
        }
    }
}
