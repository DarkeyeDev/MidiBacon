using System;

namespace MidiBacon
{
    /// <summary>
    /// This meta event defines the lyrics in a song and are usually used to define a syllable or
    /// group of works per quarter note. This event can be used as an equivalent of sheet music
    /// lyrics or for implementing a karaoke-style system.
    /// </summary>
    public class LyricsEvent : BaseMetaEvent
    {
        public LyricsEvent(String text)
        {
            this.Type = MetaEventType.Lyrics;
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