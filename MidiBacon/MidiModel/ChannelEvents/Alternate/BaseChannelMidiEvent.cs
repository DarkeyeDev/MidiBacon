using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiBacon
{
    public abstract class BaseChannelMidiEvent : MidiEvent
    {
        private byte _type;

        public byte Type
        {
            get { return _type; }
            set { _type = value; }
        }

        private byte _channel;

        public byte Channel
        {
            get { return _channel; }
            set { _channel = value; }
        }

        public virtual void ConvertToBinary(System.IO.BinaryWriter writer)
        {
            UtilBinary.WriteVariableLengthValue(writer, this.DeltaTime); //use variable length binary
            UtilBinary.WriteValue(writer, UtilBinary.CombineBytes((byte)this.Type, this.Channel));
        }

        public virtual void ConvertFromBinary(System.IO.BinaryReader reader)
        {
            byte[] typeAndChannelBytes = UtilBinary.SplitByte(reader.ReadByte());
            this.Type = typeAndChannelBytes[0];
            this.Channel = typeAndChannelBytes[1];
            //this.Parameter1 = reader.ReadByte();
            //this.Parameter2 = reader.ReadByte();
        }

        public uint GetSizeInBytes()
        {
            return UtilBinary.GetLengthOfVariableLengthValue(this.DeltaTime) + sizeof(byte) + sizeof(byte) + sizeof(byte);
        }
    }
}
