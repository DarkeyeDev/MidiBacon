namespace MidiBacon
{
    public enum ChannelEventType : byte
    {
        //Unknown = 0x00,
        NoteOff = 0x8,

        NoteOn = 0x9,
        NoteAfterTouch = 0xA,
        Controller = 0xB,
        ProgramChange = 0xC,
        ChannelAfterTouch = 0xD,
        PitchBend = 0xE//,
        //Meta = 0xFF
    }
}