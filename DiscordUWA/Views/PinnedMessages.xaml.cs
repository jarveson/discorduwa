using DiscordUWA.Common;
using DiscordUWA.Models;
using DiscordUWA.ViewModels;
using System.Collections.Generic;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace DiscordUWA.Views {
    public sealed partial class PinnedMessages : BindablePage {
        public PinnedMessagesViewModel Vm {
            get {
                return (PinnedMessagesViewModel)DataContext;
            }
        }

        public PinnedMessages() {
            this.InitializeComponent();
        }
    }
}