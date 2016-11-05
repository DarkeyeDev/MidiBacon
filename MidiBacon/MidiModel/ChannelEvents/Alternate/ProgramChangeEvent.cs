using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiBacon.MidiModel
{
    public class ProgramChangeEvent : BaseChannelMidiEvent
    {
        public ProgramChangeEvent()
        {
            this.Type = 0xC;
        }

        private byte _programNumber;

        public byte ProgramNumber
        {
            get { return _programNumber; }
            set { _programNumber = value; }
        }

        public override void ConvertToBinary(System.IO.BinaryWriter writer)
        {
            base.ConvertToBinary(writer);
            UtilBinary.WriteValue(writer, this.ProgramNumber);
        }
    }
}
