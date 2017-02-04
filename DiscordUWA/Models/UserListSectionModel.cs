using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordUWA.Models {
    public class UserListSectionModel {
        public string RoleName;
        public IEnumerable<UserListModel> Users;
        public UserListSectionModel(string role, IEnumerable<UserListModel> userList) {
            RoleName = role;
            Users = userList;
        }
    }
}
