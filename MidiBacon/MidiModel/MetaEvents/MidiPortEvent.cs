namespace MidiBacon
{
    /// <summary>
    /// </summary>
    public class MidiPortEvent : BaseMetaEvent
    {
        public MidiPortEvent(byte port)
        {
            this.Type = MetaEventType.MidiPort;
            this.Port = port;
        }

        public override uint DataLength
        {
            get
            {
                return 1;
            }
        }

        private byte _port;

        public byte Port
        {
            get { return _port; }
            set { _port = value; }
        }

        public override void ConvertToBinary(System.IO.BinaryWriter writer)
        {
            if (this.DataLength > 0)
            {
                base.ConvertToBinary(writer);
                UtilBinary.WriteValue(writer, this.Port);
            }
        }
    }
}