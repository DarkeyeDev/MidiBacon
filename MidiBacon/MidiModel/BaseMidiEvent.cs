namespace MidiBacon
{
    public abstract class BaseMidiEvent
    {
        public BaseMidiEvent()
        {
            this.DeltaTicks = 0;
            this.Position = new MidiTime();
        }

        private MidiFile _file;

        public MidiFile File
        {
            get { return _file; }
            set { _file = value; }
        }

        private uint _deltaTicks;

        /// <summary>
        /// stored as a variable length binary value. therefore the number '1' can tbe stored with 1
        /// byte like 01, rather than 00 00 00 01. this saves lots of space in the midi file
        /// </summary>
        public uint DeltaTicks
        {
            get { return _deltaTicks; }
            set { _deltaTicks = value; }
        }

        private uint _totalTicks;

        public uint TotalTicks
        {
            get { return _totalTicks; }
            set { _totalTicks = value; }
        }

        public virtual void ConvertToBinary(System.IO.BinaryWriter writer)
        {
            UtilBinary.WriteVariableLengthValue(writer, this.DeltaTicks);      //use variable length binary
        }

        private MidiTime _position;

        public MidiTime Position
        {
            get { return _position; }
            set { _position = value; }
        }
    }
}