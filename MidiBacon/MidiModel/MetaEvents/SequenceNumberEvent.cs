namespace MidiBacon
{
    /// <summary>
    /// This meta event defines the pattern number of a Type 2 MIDI file or the number of a sequence
    /// in a Type 0 or Type 1 MIDI file. This meta event should always have a delta time of 0 and
    /// come before all MIDI Channel Events and non-zero delta time events.
    /// </summary>
    public class SequenceNumberEvent : BaseMetaEvent
    {
        public SequenceNumberEvent(byte numberMSB, byte numberLSB)
        {
            this.Type = MetaEventType.SequenceNumber;
        }

        public override uint DataLength
        {
            get
            {
                return 2;
            }
        }

        private byte _numbermsb;

        public byte NumberMSB
        {
            get { return _numbermsb; }
            set { _numbermsb = value; }
        }

        private byte _numberlsb;

        public byte NumberLSB
        {
            get { return _numberlsb; }
            set { _numberlsb = value; }
        }

        public override void ConvertToBinary(System.IO.BinaryWriter writer)
        {
            if (this.DataLength > 0)
            {
                base.ConvertToBinary(writer);
                UtilBinary.WriteValue(writer, this.NumberMSB);
                UtilBinary.WriteValue(writer, this.NumberLSB);
            }
        }
    }
}