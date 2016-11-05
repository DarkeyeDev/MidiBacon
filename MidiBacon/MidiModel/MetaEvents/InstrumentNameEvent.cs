using System;

namespace MidiBacon
{
    /// <summary>
    /// This meta event defines the name of an instrument being used in the current track chunk. This
    /// event can be used with the MIDI Channel Prefix meta event to define which instrument is being
    /// used on a specific channel.
    /// </summary>
    public class InstrumentNameEvent : BaseMetaEvent
    {
        public InstrumentNameEvent(String text)
        {
            this.Type = MetaEventType.InstrumentName;
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