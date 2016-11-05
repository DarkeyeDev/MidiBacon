namespace MidiBacon
{
    /// <summary>
    /// This meta event is used to specify the SMPTE starting point offset from the beginning of the
    /// track. It is defined in terms of hours, minutes, seconds, frames and sub-frames (always 100
    /// sub-frames per frame, no matter what sub-division is specified in the MIDI header chunk). The
    /// byte used to specify the hour offset also specifies the frame rate in the following format:
    /// 0rrhhhhhh where rr is two bits for the frame rate where 00=24 fps, 01=25 fps, 10=30 fps (drop
    /// frame), 11=30 fps and hhhhhh is six bits for the hour (0-23). The hour byte's top bit is
    /// always 0. The frame byte's possible range depends on the encoded frame rate in the hour byte.
    /// A 25 fps frame rate means that a maximum value of 24 may be set for the frame byte.
    /// </summary>
    public class SMPTEOffSetEvent : BaseMetaEvent
    {
        public SMPTEOffSetEvent(byte hours = 0, byte minutes = 0, byte frames = 0, byte subframes = 0)
        {
            this.Type = MetaEventType.SMPTEOffSet;
        }

        public override uint DataLength
        {
            get
            {
                return 5;
            }
        }

        private byte _hours;

        public byte Hours
        {
            get { return _hours; }
            set { _hours = value; }
        }

        private byte _mins;

        public byte Minutes
        {
            get { return _mins; }
            set { _mins = value; }
        }

        private byte _seconds;

        public byte Seconds
        {
            get { return _seconds; }
            set { _seconds = value; }
        }

        private byte _frames;

        public byte Frames
        {
            get { return _frames; }
            set { _frames = value; }
        }

        private byte _subFrames;

        public byte SubFrames
        {
            get { return _subFrames; }
            set { _subFrames = value; }
        }

        public override void ConvertToBinary(System.IO.BinaryWriter writer)
        {
            if (this.DataLength > 0)
            {
                base.ConvertToBinary(writer);
                UtilBinary.WriteValue(writer, this.Hours);
                UtilBinary.WriteValue(writer, this.Minutes);
                UtilBinary.WriteValue(writer, this.Seconds);
                UtilBinary.WriteValue(writer, this.Frames);
                UtilBinary.WriteValue(writer, this.SubFrames);
            }
        }
    }
}