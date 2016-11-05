using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiBacon.MidiModel
{
    public class NoteOnEvent : BaseChannelMidiEvent
    {

        public NoteOnEvent()
        {
            this.Type = 0x9;
        }

        private byte _noteNumber;

        public byte NoteNumber
        {
            get { return _noteNumber; }
            set { _noteNumber = value; }
        }

        private byte _velocity;

        public byte Velocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        }

        public override void ConvertToBinary(System.IO.BinaryWriter writer)
        {
            base.ConvertToBinary(writer);
            UtilBinary.WriteValue(writer, this.NoteNumber);
            UtilBinary.WriteValue(writer, this.Velocity);
        }
    }
}
