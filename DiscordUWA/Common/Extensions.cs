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
        
        public static string GetAvatarUrlOrDefault(this Discord.IUser user) {
            static string server = "https://discordapp.com";
            static string[] DefaultAvatars = new string[] {
                "/assets/6debd47ed13483642cf09e832ed0bc1b.png",
                "/assets/322c936a8c8be1b803cd94861bdfa868.png",
                "/assets/dd4dbc0016779df1378e7812eabaa04d.png",
                "/assets/0e291f67c9274a1abdddeb3fd919cbaa.png",
                "/assets/1cbd08c76f8af6dddce02c5138971129.png"
            };

            static string DefaultBotAvatar = "/assets/f78426a064bc9dd24847519259bc42af.png";

            if (string.IsNullOrEmpty(user.AvatarUrl)) {
                if (user.IsBot)
                    return server + DefaultBotAvatar;
                return server + DefaultAvatars[user.DiscriminatorValue % DefaultAvatars.Length];
            }
            return user.AvatarUrl;
        }
    }
}
