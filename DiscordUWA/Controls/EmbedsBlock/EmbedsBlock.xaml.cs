using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Discord;
using System.Diagnostics;

namespace DiscordUWA.Controls {

    public sealed class EmbedsBlock : Control {

        /// <summary>
        /// The root element for our rendering.
        /// </summary>
        private Border _rootElement;

        public IEnumerable<IEmbed> EmbedsList {
            get { return (IEnumerable<IEmbed>)GetValue(EmbedsProperty); }
            set { SetValue(EmbedsProperty, value); }
        }

        public static readonly DependencyProperty EmbedsProperty = DependencyProperty.Register(
            nameof(EmbedsList),
            typeof(IEnumerable<IEmbed>),
            typeof(EmbedsBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        private static void OnPropertyChangedStatic(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var instance = d as EmbedsBlock;

            // Defer to the instance method.
            instance?.OnPropertyChanged(d, e.Property);
        }

        public EmbedsBlock() {
            // Set our style.
            DefaultStyleKey = typeof(EmbedsBlock);
        }

        /// <inheritdoc />
        protected override void OnApplyTemplate() {
            // Grab our root
            _rootElement = GetTemplateChild("RootElement") as Border;

            // And make sure to render any markdown we have.
            RenderEmbeds();
        }

        private void OnPropertyChanged(DependencyObject d, DependencyProperty prop) {
            RenderEmbeds();
        }

        private void RenderEmbeds() {
            // Make sure we have something to parse.
            if (EmbedsList == null) {
                return;
            }

            // Leave if we don't have our root yet.
            if (_rootElement == null) {
                return;
            }

            try {
                // Now try to display it
                var renderer = new EmbedsRenderer(EmbedsList);
                _rootElement.Child = renderer.Render();
            }
            catch (Exception ex) {
                Debug.WriteLine("Error while parsing and rendering: " + ex.Message);
            }

        }
    }
}
