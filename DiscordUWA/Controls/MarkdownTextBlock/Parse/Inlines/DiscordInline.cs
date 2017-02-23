using System;
using System.Collections.Generic;
using DiscordUWA.Controls.Markdown;

namespace DiscordUWA.Controls.Markdown.Parse
{
    /// <summary>
    /// Represents a type of hyperlink where the text can be different from the target URL.
    /// </summary>
    internal class DiscordInline : MarkdownInline, ILinkElement, IInlineLeaf
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MarkdownLinkInline"/> class.
        /// </summary>
        public DiscordInline()
            : base(MarkdownInlineType.Discord)
        {
        }

        /// <summary>
        /// Gets or sets the link URL.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets a tooltip to display on hover.
        /// </summary>
        public string Tooltip { get; set; }


        public ulong ID { get; set; } = 0L;

        public string Text { get; set; }

        /// <summary>
        /// Returns the chars that if found means we might have a match.
        /// </summary>
        internal static void AddTripChars(List<Helpers.Common.InlineTripCharHelper> tripCharHelpers)
        {}

        internal static Helpers.Common.InlineParseResult Parse(string markdown, int start, int maxEnd)
        {
            int innerStart = start + 1;
            int pos = -1;

            if ((maxEnd - innerStart) > 2) {
                char char1 = markdown[innerStart];
                if (char1 == '@') {
                    var char2 = markdown[innerStart+1];
                    if (char2 == '!') {
                        // nickname
                        pos = innerStart + 2;
                    }
                    else if (char2 == '&') {
                        // role
                        pos = innerStart + 2;
                    }
                    else {
                        pos = innerStart + 1;
                    }
                }
                else if (char1 == '#') {
                    // channel
                    pos = innerStart + 1;
                }
                // else it can be custom emote,
                // todo: custom emote
            }

            if (pos == -1)
            {
                return null;
            }

            // make sure this has ending bracket
            string text = string.Empty;
            int innerEnd = markdown.IndexOf('>');
            if (innerEnd == -1)
                return null;
            // check if its all numeric before our ':' which is 'custom' to our discord tag
            // this avoids passing or hooking id lookups this deep in a control
            for (int i = pos; i < innerEnd; ++i ) {
                if (char == ':')
                    break;
                if (!char.IsNumber(markdown[i]))
                    return null;
            }

            // remove end bracket
            innerEnd--;

            // extract true id/name string
            int nameStart = markdown.IndexOf(':');
            string idStr = string.Empty;
            string text = string.Empty;
            if (nameStart != -1) {
                idStr = markdown.Substring(pos, nameStart - innerStart);
                text = markdown.Substring(nameStart, innerEnd - nameStart);
            }
            else {
                idStr = markdown.Substring(pos, innerEnd - innerStart);
                text = idStr;
            }
            UInt64.TryParse(idStr, out ulong id);

            // We found a regular stand-alone link.
            return new Helpers.Common.InlineParseResult(
                new DiscordInline {
                    Url = idStr,
                    Tooltip = "",
                    ID = id,
                    Text = text,
                }, 
                start, 
                innerEnd+2
            );
        }

        public override string ToString()
        {
            if (Text == null)
            {
                return "@" + base.ToString();
            }

            return "@" + Text;
        }
    }
}
