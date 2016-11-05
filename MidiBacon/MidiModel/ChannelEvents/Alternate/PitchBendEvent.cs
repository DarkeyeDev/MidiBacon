using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiBacon.MidiModel
{
    public class PitchBendEvent : BaseChannelMidiEvent
    {
        public PitchBendEvent()
        {
            this.Type = 0xE;
        }

        private byte _pitchValueMSB;

        public byte PitchValueMSB
        {
            get { return _pitchValueMSB; }
            set { _pitchValueMSB = value; }
        }

        private byte _pitchValueLSB;

        public byte PitchValueLSB
        {
            get { return _pitchValueLSB; }
            set { _pitchValueLSB = value; }
        }
        
        public override void ConvertToBinary(System.IO.BinaryWriter writer)
        {
            base.ConvertToBinary(writer);
            UtilBinary.WriteValue(writer, this.PitchValueMSB);
            UtilBinary.WriteValue(writer, this.PitchValueLSB);
        }
    }
}
