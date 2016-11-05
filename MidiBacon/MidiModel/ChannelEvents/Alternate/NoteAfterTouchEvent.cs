using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiBacon.MidiModel
{
    public class NoteAfterTouchEvent : BaseChannelMidiEvent
    {
        public NoteAfterTouchEvent()
        {
            this.Type = 0xA;
        }

        private byte _noteNumber;

        public byte NoteNumber
        {
            get { return _noteNumber; }
            set { _noteNumber = value; }
        }

        private byte _afterTouchValue;

        public byte AfterTouchValue
        {
            get { return _afterTouchValue; }
            set { _afterTouchValue = value; }
        }

        public override void ConvertToBinary(System.IO.BinaryWriter writer)
        {
            base.ConvertToBinary(writer);
            UtilBinary.WriteValue(writer, this.NoteNumber);
            UtilBinary.WriteValue(writer, this.AfterTouchValue);
        }

    }
}
