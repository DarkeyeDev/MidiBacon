namespace MidiBacon
{
    /// <summary>
    /// This meta event is used to specify the key (number of sharps or flats) and scale (major or
    /// minor) of a sequence. A positive value for the key specifies the number of sharps and a
    /// negative value specifies the number of flats. A value of 0 for the scale specifies a major
    /// key and a value of 1 specifies a minor key.
    /// </summary>
    public class KeySignatureEvent : BaseMetaEvent
    {
        public KeySignatureEvent(byte key, byte scale)
        {
            this.Type = MetaEventType.KeySignature;
            this.Key = key;
            this.Scale = scale;
        }

        public override uint DataLength
        {
            get
            {
                return 2;
            }
        }

        private byte _key;

        public byte Key
        {
            get { return _key; }
            set { _key = value; }
        }

        private byte _scale;

        public byte Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }

        public override void ConvertToBinary(System.IO.BinaryWriter writer)
        {
            base.ConvertToBinary(writer);
            UtilBinary.WriteValue(writer, this.Key);
            UtilBinary.WriteValue(writer, this.Scale);
        }
    }
}