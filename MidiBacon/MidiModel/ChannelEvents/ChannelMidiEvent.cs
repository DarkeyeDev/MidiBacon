using System.Text;

namespace MidiBacon
{
    public class ChannelMidiEvent : BaseMidiEvent
    {
        public ChannelMidiEvent()
            : base()
        {
        }

        public ChannelMidiEvent(ChannelEventType type, byte channel, byte param1, byte param2, uint deltaTime)
            : this()
        {
            this.DeltaTicks = deltaTime;
            this.Type = type;
            this.Channel = channel;
            this.Parameter1 = param1;
            this.Parameter2 = param2;
            //this.EventLengthInTicks = 0;
        }

        //public ChannelMidiEvent(ChannelMidiEvent eventToCopy)
        //    :this(eventToCopy.Type, eventToCopy.Channel, eventToCopy.Parameter1, eventToCopy.Parameter2, eventToCopy.DeltaTicks)
        //{
        //    this.Position = eventToCopy.Position;
        //    this.TotalTicks = eventToCopy.TotalTicks;
        //    this.EventLengthInTicks = eventToCopy.EventLengthInTicks;
        //    this.AssociatedEvent = eventToCopy.AssociatedEvent;
        //}

        private ChannelEventType _type;

        public ChannelEventType Type
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

        private byte _parameter1;

        public byte Parameter1
        {
            get { return _parameter1; }
            set { _parameter1 = value; }
        }

        private byte _parameter2;

        public byte Parameter2
        {
            get { return _parameter2; }
            set { _parameter2 = value; }
        }

        private ChannelMidiEvent _associatedEvent;

        public ChannelMidiEvent AssociatedEvent
        {
            get { return _associatedEvent; }
            set { _associatedEvent = value; }
        }

        //private uint _eventLengthInTicks;

        //public uint EventLengthInTicks
        //{
        //    get { return _eventLengthInTicks; }
        //    set { _eventLengthInTicks = value; }
        //}

        //public static ChannelMidiEvent CreateNoteOnOffChannelMidiEvent(byte channel, byte noteNumber, byte velocity, uint deltaTime, uint duration)
        //{
        //    return new ChannelMidiEvent(ChannelEventType.NoteOff, channel, noteNumber, velocity, deltaTime);
        //}

        public static ChannelMidiEvent CreateNoteOffChannelMidiEvent(byte channel, byte noteNumber, uint deltaTime)
        {
            return new ChannelMidiEvent(ChannelEventType.NoteOff, channel, noteNumber, 0, deltaTime);
        }

        public static ChannelMidiEvent CreateNoteOnChannelMidiEvent(byte channel, byte noteNumber, byte velocity, uint deltaTime)
        {
            return new ChannelMidiEvent(ChannelEventType.NoteOn, channel, noteNumber, velocity, deltaTime);
        }

        //public static ChannelMidiEvent CreateNoteAfterTouchChannelMidiEvent(byte channel, byte noteNumber, byte aftertouchValue, uint deltaTime)
        //{
        //    return new ChannelMidiEvent(ChannelEventType.NoteAfterTouch, channel, noteNumber, aftertouchValue, deltaTime);
        //}

        //public static ChannelMidiEvent CreateControllerChannelMidiEvent(byte channel, byte controllerNumber, byte controllerValue, uint deltaTime)
        //{
        //    return new ChannelMidiEvent(ChannelEventType.Controller, channel, controllerNumber, controllerValue, deltaTime);
        //}

        //public static ChannelMidiEvent CreateProgramChangehannelMidiEvent(byte channel, byte programNumber, uint deltaTime)
        //{
        //    return new ChannelMidiEvent(ChannelEventType.ProgramChange, channel, programNumber, 0, deltaTime);
        //}

        //public static ChannelMidiEvent CreateChannelAftertouchChannelMidiEvent(byte channel, byte value, uint deltaTime)
        //{
        //    return new ChannelMidiEvent(ChannelEventType.ChannelAfterTouch, channel, value, 0, deltaTime);
        //}

        //public static ChannelMidiEvent CreatePitchBendChannelMidiEvent(byte channel, byte pitchValueLSB, byte pitchValueMSB, uint deltaTime)
        //{
        //    return new ChannelMidiEvent(ChannelEventType.PitchBend, channel, pitchValueLSB, pitchValueMSB, deltaTime);
        //}

        public override void ConvertToBinary(System.IO.BinaryWriter writer)
        {
            base.ConvertToBinary(writer);

            //if (this.Parameter2 == 0 && this.Type == ChannelEventType.NoteOn)
            //    UtilBinary.WriteValue(writer, UtilBinary.CombineBytes((byte)ChannelEventType.NoteOff, this.Channel));
            //else
            UtilBinary.WriteValue(writer, UtilBinary.CombineBytes((byte)this.Type, this.Channel));

            UtilBinary.WriteValue(writer, this.Parameter1);
            UtilBinary.WriteValue(writer, this.Parameter2);
        }

        public uint GetSizeInBytes()
        {
            return UtilBinary.GetLengthOfVariableLengthValue(this.DeltaTicks) + sizeof(byte) + sizeof(byte) + sizeof(byte);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("-------------------------------------------------");
            sb.AppendFormat("DeltaTicks: {0}", this.DeltaTicks).AppendLine();
            sb.AppendFormat("TotalTicks: {0}", this.TotalTicks).AppendLine();
            sb.AppendFormat("Type: {0}", this.Type).AppendLine();
            sb.AppendFormat("Channel: {0}", this.Channel).AppendLine();
            sb.AppendFormat("Param1: {0}", this.Parameter1).AppendLine();
            sb.AppendFormat("Param2: {0}", this.Parameter2).AppendLine();
            sb.AppendFormat("Measure:Beats:Ticks: {0}", this.Position).AppendLine();
            return sb.ToString();
        }
    }
}