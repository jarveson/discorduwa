using DiscordUWA.Common;
using DiscordUWA.Interfaces;
using System;
using System.Windows.Input;

namespace DiscordUWA.ViewModels {
    class ChatViewModel : BindableBase {
        public ICommand SendChatMessage { protected set; get; }

        private string chatMessages;
        public string ChatMessages {
            get { return this.ChatMessages; }
            set { SetProperty<string>(ref chatMessages, value); }
        }

        private string chatText;
        public string ChatText {
            get { return this.chatText; }
            set { SetProperty<string>(ref chatText, value); }
        }

        public ChatViewModel() {
            /*this.SendChatMessage = new DelegateCommand(async () => {
                //App.Current.Locator.DiscordClient.
            });*/
        }
    }
}
