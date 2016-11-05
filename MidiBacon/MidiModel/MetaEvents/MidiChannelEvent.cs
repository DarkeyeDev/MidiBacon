namespace MidiBacon
{
    /// <summary>
    /// This meta event associates a MIDI channel with following meta events. It's effect is
    /// terminated by another MIDI Channel Prefix event or any non- Meta event. It is often used
    /// before an Instrument Name Event to specify which channel an instrument name represents
    /// </summary>
    public class MidiChannelEvent : BaseMetaEvent
    {
        public MidiChannelEvent(byte channel)
        //:base()
        {
            this.Type = MetaEventType.MidiChannel;
            this.Channel = channel;
        }

        public override uint DataLength
        {
            get
            {
                return 1;
            }
        }

        private byte _channel;

        public byte Channel
        {
            get { return _channel; }
            set { _channel = value; }
        }

        public override void ConvertToBinary(System.IO.BinaryWriter writer)
        {
            if (this.DataLength > 0)
            {
                base.ConvertToBinary(writer);
                UtilBinary.WriteValue(writer, this.Channel);
            }
        }
    }
}