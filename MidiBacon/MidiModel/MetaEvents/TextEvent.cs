using System;

namespace MidiBacon
{
    /// <summary>
    /// This meta event defines some text which can be used for any reason including track notes,
    /// comments, etc. The text string is usually ASCII text, but may be any character (0x00-0xFF).
    /// </summary>
    public class TextEvent : BaseMetaEvent
    {
        public TextEvent(String text)
        {
            this.Type = MetaEventType.TextEvent;
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