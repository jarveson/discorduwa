using Discord;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace DiscordUWA.Controls {
    internal class EmbedsRenderer {

        private readonly IEnumerable<IEmbed> _embeds;

        public EmbedsRenderer(IEnumerable<IEmbed> embeds) {
            _embeds = embeds;
        }

        public UIElement Render() {
            var stackPanel = new StackPanel();
            foreach (var embed in _embeds) {
                RenderEmbed(embed, stackPanel.Children);
            }

            return stackPanel;
        }

        private void RenderEmbed(IEmbed embed, UIElementCollection blockUIElementCollection) {
            switch (embed.Type) {
                case "link": {
                        if (!embed.Thumbnail.HasValue) return;
                        var result = new Windows.UI.Xaml.Controls.Image {
                            HorizontalAlignment = HorizontalAlignment.Left,
                            MaxHeight = Math.Min(75, embed.Thumbnail.Value.Height.Value),
                            Source = new BitmapImage {
                                DecodePixelType = DecodePixelType.Logical,
                                DecodePixelHeight = Math.Min(75, embed.Thumbnail.Value.Height.Value),
                                UriSource = new Uri(embed.Thumbnail.Value.Url),
                            },
                        };
                        blockUIElementCollection.Add(result);
                    }
                    break;
                case "gifv": {
                        var result = new MediaElement {
                            IsLooping = true,
                            AreTransportControlsEnabled = false,
                            Source = new Uri(embed.Video.Value.Url),
                            MaxHeight = Math.Min(250, embed.Video.Value.Height.Value),
                            MaxWidth = embed.Video.Value.Width.Value,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            IsMuted = true,
                        };
                        blockUIElementCollection.Add(result);
                    }
                    break;
                case "image": {
                        var result = new Windows.UI.Xaml.Controls.Image {
                            HorizontalAlignment = HorizontalAlignment.Left,
                            MaxHeight = Math.Min(250, embed.Thumbnail.Value.Height.Value),
                            Source = new BitmapImage {
                                DecodePixelType = DecodePixelType.Logical,
                                DecodePixelHeight = Math.Min(250, embed.Thumbnail.Value.Height.Value),
                                UriSource = new Uri(embed.Url),
                            },
                        };
                        blockUIElementCollection.Add(result);
                    }
                    break;
                default:
                    Debug.WriteLine("Unknown Embed Type -- " + embed.Type);
                    break;
            }
        }
    }
}