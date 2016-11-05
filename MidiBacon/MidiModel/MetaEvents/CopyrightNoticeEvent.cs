using System;

namespace MidiBacon
{
    /// <summary>
    /// This meta event defines copyright information including the copyright symbol © (0xA9), the
    /// year and the author. This meta event should always be in the first track chunk, have a delta
    /// time of 0 and come before all MIDI Channel Events and non-zero delta time events.
    /// </summary>
    public class CopyrightNoticeEvent : BaseMetaEvent
    {
        public CopyrightNoticeEvent(String text)
        {
            this.Type = MetaEventType.CopyrightNotice;
            this.Text = text;
        }

        private String _text;

        public String Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public override uint DataLength
        {
            get
            {
                return (uint)this.Text.Length;
            }
        }

        public override void ConvertToBinary(System.IO.BinaryWriter writer)
        {
            if (this.DataLength > 0)
            {
                base.ConvertToBinary(writer);
                UtilBinary.WriteValue(writer, this.Text.ToCharArray());
            }
        }
    }
}