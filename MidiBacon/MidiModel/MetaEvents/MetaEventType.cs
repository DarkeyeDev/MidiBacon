namespace MidiBacon
{
    public enum MetaEventType : byte
    {
        SequenceNumber = 0x00,
        TextEvent = 0x01,
        CopyrightNotice = 0x02,
        TrackName = 0x03,
        InstrumentName = 0x04,
        Lyrics = 0x05,
        Marker = 0x06,
        CuePoint = 0x07,
        DeviceName = 0x09,
        MidiChannel = 0x20,
        MidiPort = 0x21,
        EndOfTrack = 0x2F,
        Tempo = 0x51,
        SMPTEOffSet = 0x54,
        TimeSignature = 0x58,
        KeySignature = 0x59,
        SequencerSpecific = 0x7F
    }
}