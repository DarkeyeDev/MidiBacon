using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiBacon.MidiModel
{
    public class ControllerEvent : BaseChannelMidiEvent
    {
        public ControllerEvent()
        {
            this.Type = 0xB;
        }

        private byte _controllerType;

        public byte ControllerType
        {
            get { return _controllerType; }
            set { _controllerType = value; }
        }

        private byte _value;

        public byte Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public override void ConvertToBinary(System.IO.BinaryWriter writer)
        {
            base.ConvertToBinary(writer);
            UtilBinary.WriteValue(writer, this.ControllerType);
            UtilBinary.WriteValue(writer, this.Value);
        }
    }
}
