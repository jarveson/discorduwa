using Discord;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace DiscordUWA.Controls {
    internal class AttachmentsRenderer {

        private readonly IEnumerable<IAttachment> _attachments;

        public AttachmentsRenderer(IEnumerable<IAttachment> attachments) {
            _attachments = attachments;
        }

        public UIElement Render() {
            var stackPanel = new StackPanel();
            foreach (var attachment in _attachments) {
                RenderAttachment(attachment, stackPanel.Children);
            }

            return stackPanel;
        }

        private void RenderAttachment(IAttachment attach, UIElementCollection blockUIElementCollection) {
            if (attach.Width.HasValue && attach.Height.HasValue) {
                var result = new Windows.UI.Xaml.Controls.Image {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    MaxHeight = Math.Min(300, attach.Height.Value),
                    MaxWidth = Math.Min(400, attach.Width.Value),
                    Source = new BitmapImage {
                        DecodePixelType = DecodePixelType.Logical,
                        DecodePixelHeight = Math.Min(300, attach.Height.Value),
                        UriSource = new Uri(attach.Url),
                    },
                };
                blockUIElementCollection.Add(result);
            }
        }
    }
}