namespace MidiBacon
{
    public abstract class BaseMetaEvent : BaseMidiEvent
    {
        public BaseMetaEvent()
            : base()
        {
            this.MetaEventId = 0xFF;
            this.DeltaTicks = 0;
        }

        private byte _metaEventId;

        public byte MetaEventId
        {
            get { return _metaEventId; }
            set { _metaEventId = value; }
        }

        private MetaEventType _type;

        public MetaEventType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public virtual uint DataLength
        {
            get
            {
                return 0;
            }
        }

        public override void ConvertToBinary(System.IO.BinaryWriter writer)
        {
            base.ConvertToBinary(writer);

            UtilBinary.WriteValue(writer, this.MetaEventId);
            UtilBinary.WriteValue(writer, (byte)this.Type);
            UtilBinary.WriteVariableLengthValue(writer, this.DataLength);    //use variable length binary
        }

        public virtual uint GetSizeInBytes()
        {
            if (this.DataLength > 0)
            {
                uint size = UtilBinary.GetLengthOfVariableLengthValue(this.DeltaTicks)    //base holds the delta ticks should always be zero for meta event
                + sizeof(byte)  //meta event id FF
                + sizeof(byte) //meta type
                + UtilBinary.GetLengthOfVariableLengthValue(this.DataLength)    //the length of the number that stores the length of the data
                + (uint)this.DataLength;    //the length of the data (set this in the concrete class)

                return size;
            }
            else
                return 0;
        }
    }
}