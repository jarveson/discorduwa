using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace DiscordUWA.Models {
    public class UserListSectionModel : UserListModel {
        public string RoleSectionName = "";

        public UserListSectionModel(string avatarUrl, string currentlyPlaying, Color statusColor, string username, Color userRoleColor, ulong id) : base(avatarUrl, currentlyPlaying, statusColor, username,userRoleColor, id){
        }

        public UserListSectionModel(string role) {
            RoleSectionName = role;
        }
    }
}
