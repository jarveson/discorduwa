using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace DiscordUWA.Models {
    public class UserListSectionModel : UserListModel {
        public string RoleSectionName = "";
        public uint NumUsers = 0;

        public UserListSectionModel(string avatarUrl, string currentlyPlaying, Color statusColor, string username, Color userRoleColor, ulong id, bool isBot) : base(avatarUrl, currentlyPlaying, statusColor, username,userRoleColor, id, isBot){
        }

        public UserListSectionModel(string role, uint numUsers) {
            RoleSectionName = role;
            NumUsers = numUsers;
        }
    }
}
