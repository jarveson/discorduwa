using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Discord;
using System.Diagnostics;

namespace DiscordUWA.Controls {

    public sealed class AttachmentsBlock : Control {

        /// <summary>
        /// The root element for our rendering.
        /// </summary>
        private Border _rootElement;

        public IEnumerable<IAttachment> AttachmentsList {
            get { return (IEnumerable<IAttachment>)GetValue(AttachmentsListProperty); }
            set { SetValue(AttachmentsListProperty, value); }
        }

        public static readonly DependencyProperty AttachmentsListProperty = DependencyProperty.Register(
            nameof(AttachmentsList),
            typeof(IEnumerable<IAttachment>),
            typeof(AttachmentsBlock),
            new PropertyMetadata(null, OnPropertyChangedStatic));

        private static void OnPropertyChangedStatic(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var instance = d as AttachmentsBlock;

            // Defer to the instance method.
            instance?.OnPropertyChanged(d, e.Property);
        }

        public AttachmentsBlock() {
            // Set our style.
            DefaultStyleKey = typeof(AttachmentsBlock);
        }

        /// <inheritdoc />
        protected override void OnApplyTemplate() {
            // Grab our root
            _rootElement = GetTemplateChild("RootElement") as Border;

            RenderAttachments();
        }

        private void OnPropertyChanged(DependencyObject d, DependencyProperty prop) {
            RenderAttachments();
        }

        private void RenderAttachments() {
            // Make sure we have something to parse.
            if (AttachmentsList == null) {
                return;
            }

            // Leave if we don't have our root yet.
            if (_rootElement == null) {
                return;
            }

            try {
                // Now try to display it
                var renderer = new AttachmentsRenderer(AttachmentsList);
                _rootElement.Child = renderer.Render();
            }
            catch (Exception ex) {
                Debug.WriteLine("Error while parsing and rendering: " + ex.Message);
            }

        }
    }
}
