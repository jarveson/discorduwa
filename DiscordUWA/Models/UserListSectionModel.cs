using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace DiscordUWA.Models {
    public class UserListSectionModel : UserListModel {
        public string RoleSectionName { get; set; } = string.Empty;
        public uint NumUsers { get; set; } = 0;
    }
}
