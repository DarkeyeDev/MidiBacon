using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiBacon.MidiModel
{
    public class ChannelAfterTouchEvent : BaseChannelMidiEvent
    {
        public ChannelAfterTouchEvent()
        {
            this.Type = 0xD;
        }

        private byte _channelNumber;

        public byte ChannelNumber
        {
            get { return _channelNumber; }
            set { _channelNumber = value; }
        }

        public override void ConvertToBinary(System.IO.BinaryWriter writer)
        {
            base.ConvertToBinary(writer);
            UtilBinary.WriteValue(writer, this.ChannelNumber);
        }
    }
}
