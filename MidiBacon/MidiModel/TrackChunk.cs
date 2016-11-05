using System;
using System.Collections.Generic;
using System.Text;

namespace MidiBacon
{
    public class TrackChunk : BaseChunk
    {
        public TrackChunk()
            : base()
        {
            this.ChunkID = new char[4] { 'M', 'T', 'r', 'k' };
            this.ChannelEvents = new List<ChannelMidiEvent>();
            this.EndOfTrack = new EndOfTrackEvent();
        }

        public void CopyMetaData(TrackChunk trackToCopy)
        {
            this.Channel = trackToCopy.Channel;
            this.Comments = trackToCopy.Comments;
            this.CopyrightNotice = trackToCopy.CopyrightNotice;
            this.CuePoint = trackToCopy.CuePoint;
            this.DeviceName = trackToCopy.DeviceName;
            this.EndOfTrack = trackToCopy.EndOfTrack;
            this.InstrumentName = trackToCopy.InstrumentName;
            this.KeySignature = trackToCopy.KeySignature;
            this.Lyrics = trackToCopy.Lyrics;
            this.Marker = trackToCopy.Marker;
            this.Port = trackToCopy.Port;
            this.SequenceNumber = trackToCopy.SequenceNumber;
            this.SMPTEOffSet = trackToCopy.SMPTEOffSet;
            this.Tempo = trackToCopy.Tempo;
            this.TimeSignature = trackToCopy.TimeSignature;
            this.TrackName = trackToCopy.TrackName;
        }

        private List<ChannelMidiEvent> _channelEvents;

        public List<ChannelMidiEvent> ChannelEvents
        {
            get { return _channelEvents; }
            set { _channelEvents = value; }
        }

        public SequenceNumberEvent SequenceNumber;// = new SequenceNumberEvent();         // = 0x00
        public TextEvent Comments;// = new TextEvent();                                  // = 0x01
        public CopyrightNoticeEvent CopyrightNotice;// = new CopyrightNoticeEvent();      // = 0x02
        public TrackNameEvent TrackName;// = new TrackNameEvent();                        // = 0x03
        public InstrumentNameEvent InstrumentName;// = new InstrumentNameEvent();         // = 0x04
        public LyricsEvent Lyrics;// = new LyricsEvent();                                 // = 0x05
        public MarkerEvent Marker;// = new MarkerEvent();                                 // = 0x06
        public CuePointEvent CuePoint;// = new CuePointEvent();                           // = 0x07
        public DeviceNameEvent DeviceName;// = new DeviceNameEvent();                     // = 0x09
        public MidiChannelEvent Channel;// = new MidiChannelEvent();                  // = 0x20
        public MidiPortEvent Port;// = new MidiPortEvent();                           // = 0x21
        public EndOfTrackEvent EndOfTrack;// = new EndOfTrackEvent();                     // = 0x2F
        public TempoEvent Tempo;// = new TempoEvent(120.0);                                    // = 0x51
        public SMPTEOffSetEvent SMPTEOffSet;// = new SMPTEOffSetEvent();                  // = 0x54
        public TimeSignatureEvent TimeSignature;// = new TimeSignatureEvent();            // = 0x58
        public KeySignatureEvent KeySignature;// = new KeySignatureEvent();               // = 0x59

        public void ConvertToBinary(System.IO.BinaryWriter writer)
        {
            UtilBinary.WriteValue(writer, this.ChunkID);
            UtilBinary.WriteValue(writer, this.GetSizeInBytes());

            ///these will only get written only if there is actually data to write.

            if (this.SequenceNumber != null)
                this.SequenceNumber.ConvertToBinary(writer);

            if (this.Comments != null)
                this.Comments.ConvertToBinary(writer);

            if (this.CopyrightNotice != null)
                this.CopyrightNotice.ConvertToBinary(writer);

            if (this.TrackName != null)
                this.TrackName.ConvertToBinary(writer);

            if (this.InstrumentName != null)
                this.InstrumentName.ConvertToBinary(writer);

            if (this.Lyrics != null)
                this.Lyrics.ConvertToBinary(writer);

            if (this.Marker != null)
                this.Marker.ConvertToBinary(writer);

            if (this.CuePoint != null)
                this.CuePoint.ConvertToBinary(writer);

            if (this.DeviceName != null)
                this.DeviceName.ConvertToBinary(writer);

            if (this.Channel != null)
                this.Channel.ConvertToBinary(writer);

            if (this.Port != null)
                this.Port.ConvertToBinary(writer);

            if (this.Tempo != null)
                this.Tempo.ConvertToBinary(writer);

            if (this.SMPTEOffSet != null)
                this.SMPTEOffSet.ConvertToBinary(writer);

            if (this.TimeSignature != null)
                this.TimeSignature.ConvertToBinary(writer);

            if (this.KeySignature != null)
                this.KeySignature.ConvertToBinary(writer);

            foreach (ChannelMidiEvent chan in this.ChannelEvents)
            {
                chan.ConvertToBinary(writer);
                //Debug.Write(chan.ToString());
            }

            this.EndOfTrack.ConvertToBinary(writer);    //mandatory and must be at end
        }

        public void ConvertFromBinary(System.IO.BinaryReader reader)
        {
            this.ChunkSize = UtilBinary.ReadUintValue(reader);

            ChannelEventType lastChannelEventType = 0;// = ChannelEventType.Unknown;
            byte lastChannelEventChannel = 0;

            while (true)
            {
                //Debug.WriteLine(reader.BaseStream.Position);

                uint deltaTicks = UtilBinary.ReadVariableLengthValue(reader);
                byte trackEventType = UtilBinary.ReadByteValue(reader);

                if (trackEventType == (byte)0xFF)
                {
                    //Debug.WriteLine("*****************Meta Event********************");

                    MetaEventType metaEventType = (MetaEventType)UtilBinary.ReadByteValue(reader);

                    //Debug.WriteLine(metaEventType);

                    int lengthOfMetaData = 0;

                    //meta event
                    switch (metaEventType)
                    {
                        case MetaEventType.SequenceNumber:
                            UtilBinary.ReadByteValue(reader);
                            this.SequenceNumber = new SequenceNumberEvent(UtilBinary.ReadByteValue(reader), UtilBinary.ReadByteValue(reader));
                            break;

                        case MetaEventType.TextEvent:
                            lengthOfMetaData = (int)UtilBinary.ReadVariableLengthValue(reader);
                            this.Comments = new TextEvent(new string(UtilBinary.ReadCharsValue(reader, lengthOfMetaData)));
                            //Debug.WriteLine(this.Comments.Text);
                            break;

                        case MetaEventType.CopyrightNotice:
                            lengthOfMetaData = (int)UtilBinary.ReadVariableLengthValue(reader);
                            this.CopyrightNotice = new CopyrightNoticeEvent(new string(UtilBinary.ReadCharsValue(reader, lengthOfMetaData)));
                            //Debug.WriteLine(this.CopyrightNotice.Text);
                            break;

                        case MetaEventType.TrackName:
                            lengthOfMetaData = (int)UtilBinary.ReadVariableLengthValue(reader);
                            this.TrackName = new TrackNameEvent(new string(UtilBinary.ReadCharsValue(reader, lengthOfMetaData)));
                            //Debug.WriteLine(this.TrackName.Text);
                            break;

                        case MetaEventType.InstrumentName:
                            lengthOfMetaData = (int)UtilBinary.ReadVariableLengthValue(reader);
                            this.InstrumentName = new InstrumentNameEvent(new string(UtilBinary.ReadCharsValue(reader, lengthOfMetaData)));
                            //Debug.WriteLine(this.InstrumentName.Text);
                            break;

                        case MetaEventType.Lyrics:
                            lengthOfMetaData = (int)UtilBinary.ReadVariableLengthValue(reader);
                            this.Lyrics = new LyricsEvent(new string(UtilBinary.ReadCharsValue(reader, lengthOfMetaData)));
                            //Debug.WriteLine(this.Lyrics.Text);
                            break;

                        case MetaEventType.Marker:
                            lengthOfMetaData = (int)UtilBinary.ReadVariableLengthValue(reader);
                            this.Marker = new MarkerEvent(new string(UtilBinary.ReadCharsValue(reader, lengthOfMetaData)));
                            //Debug.WriteLine(this.Marker.Text);
                            break;

                        case MetaEventType.CuePoint:
                            lengthOfMetaData = (int)UtilBinary.ReadVariableLengthValue(reader);
                            this.CuePoint = new CuePointEvent(new string(UtilBinary.ReadCharsValue(reader, lengthOfMetaData)));
                            //Debug.WriteLine(this.CuePoint.Text);
                            break;

                        case MetaEventType.DeviceName:
                            lengthOfMetaData = (int)UtilBinary.ReadVariableLengthValue(reader);
                            this.DeviceName = new DeviceNameEvent(new string(UtilBinary.ReadCharsValue(reader, lengthOfMetaData)));
                            //Debug.WriteLine(this.DeviceName.Text);
                            break;

                        case MetaEventType.MidiChannel:
                            UtilBinary.ReadByteValue(reader);
                            this.Channel = new MidiChannelEvent(UtilBinary.ReadByteValue(reader));
                            //Debug.WriteLine(this.Channel.Channel);
                            break;

                        case MetaEventType.MidiPort:
                            UtilBinary.ReadByteValue(reader);
                            this.Port = new MidiPortEvent(UtilBinary.ReadByteValue(reader));
                            break;

                        case MetaEventType.EndOfTrack:
                            UtilBinary.ReadByteValue(reader);
                            //this.EndOfTrack = new EndOfTrackEvent();
                            //this.EndOfTrack.DeltaTicks = deltaTicks;
                            //Debug.WriteLine(this.EndOfTrack.ToString());
                            return;
                            break;

                        case MetaEventType.Tempo:
                            UtilBinary.ReadByteValue(reader);
                            uint microSecondsPerQuarterNote = UtilBinary.Read3ByteInteger(reader);
                            this.Tempo = new TempoEvent(microSecondsPerQuarterNote, this.TimeSignature.Denominator);
                            break;

                        case MetaEventType.SMPTEOffSet:
                            UtilBinary.ReadByteValue(reader);
                            this.SMPTEOffSet = new SMPTEOffSetEvent(UtilBinary.ReadByteValue(reader), UtilBinary.ReadByteValue(reader), UtilBinary.ReadByteValue(reader), UtilBinary.ReadByteValue(reader));
                            break;

                        case MetaEventType.TimeSignature:
                            UtilBinary.ReadByteValue(reader);
                            this.TimeSignature = new TimeSignatureEvent();
                            this.TimeSignature.Numerator = UtilBinary.ReadByteValue(reader);
                            this.TimeSignature.Denominator = (byte)Math.Pow(2, UtilBinary.ReadByteValue(reader));
                            this.TimeSignature.MetronomePulse = UtilBinary.ReadByteValue(reader);
                            this.TimeSignature.ThirtySecondNotes = UtilBinary.ReadByteValue(reader);
                            //Debug.WriteLine(this.TimeSignature.ToString());
                            //this.Tempo = new TempoEvent(this.Tempo.MicroSecondsPerQuarterNote, this.TimeSignature.Denominator);
                            break;

                        case MetaEventType.KeySignature:
                            UtilBinary.ReadByteValue(reader);
                            this.KeySignature = new KeySignatureEvent(UtilBinary.ReadByteValue(reader), UtilBinary.ReadByteValue(reader));
                            break;

                        case MetaEventType.SequencerSpecific:
                            throw new NotImplementedException();
                            break;

                        default:
                            break;
                    }
                }
                else
                {
                    //Debug.WriteLine("---------------------");
                    //Debug.WriteLine(deltaTime);

                    //Debug.WriteLine(UtilBinary.ReadByteValue(reader));
                    //Debug.WriteLine(UtilBinary.ReadByteValue(reader));
                    //Debug.WriteLine(UtilBinary.ReadByteValue(reader));

                    ChannelMidiEvent chan = new ChannelMidiEvent();

                    chan.DeltaTicks = deltaTicks;
                    chan.File = this.File;

                    byte[] typeAndChannelBytes = UtilBinary.SplitByte(trackEventType);
                    byte typeOrData = typeAndChannelBytes[0];

                    if (Enum.IsDefined(typeof(ChannelEventType), typeOrData))
                    {
                        chan.Type = (ChannelEventType)typeOrData;
                        lastChannelEventType = chan.Type;

                        chan.Channel = typeAndChannelBytes[1];
                        lastChannelEventChannel = chan.Channel;

                        chan.Parameter1 = UtilBinary.ReadByteValue(reader);
                        chan.Parameter2 = UtilBinary.ReadByteValue(reader);
                    }
                    else
                    {
                        chan.Type = lastChannelEventType;
                        chan.Channel = lastChannelEventChannel;
                        chan.Parameter1 = trackEventType;
                        chan.Parameter2 = UtilBinary.ReadByteValue(reader);
                    }

                    if (chan.Type == ChannelEventType.NoteOn && chan.Parameter2 == 0)
                        chan.Type = ChannelEventType.NoteOff;

                    //Debug.WriteLine(chan.Type.ToString());
                    //Debug.WriteLine(chan.Channel.ToString());

                    //this.File.Tracks[(int)chan.Channel].ChannelEvents.Add(chan);
                    this.ChannelEvents.Add(chan);

                    //Debug.Write(chan.ToString());
                }
            }
        }

        public void ConvertEventsToNotes()
        {
            int eventCounter = 0;

            while (eventCounter < this.ChannelEvents.Count)
            {
                ChannelMidiEvent currentEvent = this.ChannelEvents[eventCounter];
                currentEvent.TotalTicks = currentEvent.DeltaTicks;

                if (eventCounter > 0)
                {
                    currentEvent.TotalTicks += this.ChannelEvents[eventCounter - 1].TotalTicks;
                }

                currentEvent.Position = new MidiTime(this.File, currentEvent.TotalTicks, this.TimeSignature);

                eventCounter++;
            }

            eventCounter = 0;

            while (eventCounter < this.ChannelEvents.Count)
            {
                ChannelMidiEvent currentEvent = this.ChannelEvents[eventCounter];

                ChannelMidiEvent nextNote;

                ///find the note off for this note on
                if (currentEvent.Type == ChannelEventType.NoteOn)
                {
                    int lookForwardPostion = eventCounter + 1;
                    while (true)
                    {
                        nextNote = this.ChannelEvents[lookForwardPostion];

                        if (nextNote.Parameter1 == currentEvent.Parameter1 && nextNote.AssociatedEvent == null && nextNote.Type == ChannelEventType.NoteOff)
                        {
                            currentEvent.AssociatedEvent = nextNote;
                            nextNote.AssociatedEvent = currentEvent;
                            //currentEvent.EventLengthInTicks = nextNote.TotalTicks - currentEvent.TotalTicks;
                            break;
                        }
                        else
                            lookForwardPostion++;
                    }
                }

                if (currentEvent.AssociatedEvent == null)
                    throw new ApplicationException("found an noteOn without an associated noteOff or vice versa");

                //if (currentEvent.Type == ChannelEventType.NoteOn && currentEvent.AveragePosition.Region == null)
                //     throw new ApplicationException("found an event with an average position without a region. We should probably completely discard these events\n" + currentEvent.ToString());

                eventCounter++;
            }

            eventCounter = 0;
            this.Notes = new List<Note>();

            while (eventCounter < this.ChannelEvents.Count)
            {
                ChannelMidiEvent currentEvent = this.ChannelEvents[eventCounter];

                if (currentEvent.Type == ChannelEventType.NoteOn)
                {
                    Note n = new Note();
                    n.NoteNumber = currentEvent.Parameter1;
                    n.Length = currentEvent.AssociatedEvent.TotalTicks - currentEvent.TotalTicks;
                    n.StartPosition = currentEvent.Position;
                    n.Velocity = currentEvent.Parameter2;

                    uint noteOnTotalTicks = currentEvent.Position.GetAsTotalTicks();
                    uint noteOffTotalTicks = currentEvent.AssociatedEvent.Position.GetAsTotalTicks();
                    uint midWayTicks = (noteOnTotalTicks + noteOffTotalTicks) / 2;
                    n.AveragePosition = new MidiTime(this.File, midWayTicks, this.TimeSignature);

                    this.Notes.Add(n);
                }

                eventCounter++;
            }
        }

        public void MakeAllNotesSameKey(byte note)
        {
            foreach (Note n in this.Notes)
            {
                n.NoteNumber = note;
            }
        }

        /// <summary>
        /// i have discovered that making the note length can causes glitching when the note is
        /// played with a DAW+VST. i think the note is just so short that not even changing the
        /// release value can help. also sometimes the notes get missed and i think this is due to
        /// playback quantization errors. the DAW misses the note because it is so short.
        /// </summary>
        /// <param name="length"></param>
        public void MakeAllNotesSameLength(uint length)
        {
            foreach (Note n in this.Notes)
            {
                n.Length = length;
                n.CalculateEndPosition();
            }
        }

        public void RemoveNotesWhereNoteStartIsNotWithinMeasure()
        {
            int noteCounter = 0;
            Note currentNote = null;

            while (noteCounter < this.Notes.Count)
            {
                currentNote = this.Notes[noteCounter];

                if (currentNote.AveragePosition.Measures != currentNote.StartPosition.Measures)
                    this.Notes.Remove(currentNote);
                else
                    noteCounter++;
            }
        }

        public void RemoveNotesWhereNoteStartIsNotWithinARegion()
        {
            int noteCounter = 0;
            Note currentNote = null;

            while (noteCounter < this.Notes.Count)
            {
                currentNote = this.Notes[noteCounter];

                if (currentNote.AveragePosition.Region == null || currentNote.StartPosition.Region == null)
                    throw new ApplicationException("note is not within a region at all and therefore this comparison is not valid");

                if (currentNote.AveragePosition.Region != currentNote.StartPosition.Region)
                    this.Notes.Remove(currentNote);
                else
                    noteCounter++;
            }
        }

        public void RemoveNotesThatDoNotLieCompletelyWithinARegion()
        {
            int noteCounter = 0;
            Note currentNote = null;

            while (noteCounter < this.Notes.Count)
            {
                currentNote = this.Notes[noteCounter];

                if (currentNote.EndPosition.Region == null)
                    this.Notes.Remove(currentNote);
                else if (currentNote.StartPosition.Region == null)
                    this.Notes.Remove(currentNote);
                else
                    noteCounter++;
            }
        }

        public void CombineOverlappingNotes()
        {
            int noteCounter = 0;
            Note currentNote = null;
            Note lastNote = null;

            while (noteCounter < this.Notes.Count)
            {
                currentNote = this.Notes[noteCounter];

                if (lastNote != null && currentNote.StartPosition.GetAsTotalTicks() < lastNote.EndPosition.GetAsTotalTicks())
                {
                    lastNote.EndPosition = currentNote.EndPosition;
                    lastNote.CalculateLength();
                    this.Notes.Remove(currentNote);
                }
                else
                {
                    noteCounter++;
                    lastNote = currentNote;
                }
            }
        }

        public void ApplyScaleToNotesRandomly(byte octave, int jump, params NoteMap[] scaleNotes)
        {
            if (octave > 10)
                throw new ArgumentOutOfRangeException("octave must be between 0 and 10");

            byte octaveSize = 12;
            byte offset = (byte)(octave * octaveSize);
            Random randy = new Random();
            byte lastNoteNumber = 0;
            int scaleNoteIndex = 0;
            foreach (Note note in this.Notes)
            {
                int min = scaleNoteIndex - jump;
                int max = scaleNoteIndex + jump;
                scaleNoteIndex = randy.Next(0, scaleNotes.Length);
                NoteMap scaleNote = scaleNotes[scaleNoteIndex];
                note.NoteNumber = (byte)(scaleNote + offset);
                lastNoteNumber = note.NoteNumber;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="octave"></param>
        /// <param name="numberOfSequences">
        /// set to 0 if you dont want to limit the number of sequences applied. i.e. just keep going
        /// till we run out of notes.
        /// </param>
        /// <param name="scaleNotes"></param>
        public void ApplyScaleToNotesInSequence(byte octave, byte numberOfSequences, bool pingPong, params NoteMap[] scaleNotes)
        {
            if (octave > 10)
                throw new ArgumentOutOfRangeException("octave must be between 0 and 10");

            //if (minOctave > maxOctave)
            //    throw new ArgumentOutOfRangeException("minOctave cannot be higher than maxOctave");

            byte octaveSize = 12;
            byte offset = (byte)(octave * octaveSize);
            Random randy = new Random();
            int scaleNoteIndex = 0;
            int sequenceCounter = 0;
            //sint direction = 1;

            List<NoteMap> convertedNotes = new List<NoteMap>(scaleNotes);//convert so we can easily reverse

            foreach (Note note in this.Notes)
            {
                NoteMap scaleNote = convertedNotes[scaleNoteIndex];
                note.NoteNumber = (byte)(scaleNote + offset);
                scaleNoteIndex++;

                if (scaleNoteIndex == convertedNotes.Count)
                {
                    ///scale has hit the top note...

                    sequenceCounter++;

                    ///if numberOfSequences = 0 then we will never hit this break statement.
                    if (sequenceCounter == numberOfSequences)
                        break;

                    if (pingPong)
                    {
                        convertedNotes.Reverse();
                    }

                    scaleNoteIndex = 0;
                }
            }
        }

        public void CombineNotesWithinXTicksOfEachOther(uint x)
        {
            int noteCounter = 0;
            Note currentNote = null;
            Note lastNote = null;

            while (noteCounter < this.Notes.Count)
            {
                currentNote = this.Notes[noteCounter];

                if (lastNote != null && (currentNote.StartPosition.GetAsTotalTicks() - lastNote.EndPosition.GetAsTotalTicks()) < x)
                {
                    lastNote.EndPosition = currentNote.EndPosition;
                    lastNote.CalculateLength();
                    this.Notes.Remove(currentNote);
                }
                else
                {
                    noteCounter++;
                    lastNote = currentNote;
                }
            }
        }

        public void ConstrainNoteEndToRegionEnd()
        {
            foreach (var note in this.Notes)
            {
                if (note.StartPosition.Region == null)
                {
                    continue;
                }

                if (note.EndPosition.GetAsTotalTicks() > note.StartPosition.Region.End.GetAsTotalTicks(this.File.Header.PulsesPerQuarterNote, this.TimeSignature))
                {
                    note.EndPosition = new MidiTime(this.File, note.StartPosition.Region.End.Measures, note.StartPosition.Region.End.Beats, note.StartPosition.Region.End.Ticks, this.TimeSignature);
                    note.CalculateLength();
                }
            }
        }

        public void ConstrainNotesToMeasureBoundaries()
        {
            foreach (var note in this.Notes)
            {
                //get start and end boundary points for this note
                MidiTime startOfBar = new MidiTime(this.File, note.AveragePosition.Measures, 1, 0, this.TimeSignature);
                MidiTime endOfBar = new MidiTime(this.File, note.AveragePosition.Measures + 1, 1, 0, this.TimeSignature);

                if (note.StartPosition.GetAsTotalTicks() < startOfBar.GetAsTotalTicks())
                {
                    //note is mostly in current bar but the note starts just before the bar start.
                    //Move the note start to the to start of the measure
                    note.StartPosition = new MidiTime(startOfBar);
                    note.CalculateEndPosition();
                }
                else if (note.EndPosition.GetAsTotalTicks() > endOfBar.GetAsTotalTicks())
                {
                    ///note is mostly in current bar but the note ends just after the bar end.
                    ///Shorten the length of the note.
                    ///This will only rarely happen because at this point the note lengths are 1 and so arent long enough to span two measures
                    note.Length = endOfBar.GetAsTotalTicks() - note.StartPosition.GetAsTotalTicks();
                    note.CalculateEndPosition();
                }
            }
        }

        /// <summary>
        /// removes notes that occur at precisely the same time and are the same note number
        /// </summary>
        public void RemoveDuplicateNotes()
        {
            int noteCounter = 0;
            Note lastNote = null;
            Note currentNote = null;

            while (noteCounter < this.Notes.Count)
            {
                currentNote = this.Notes[noteCounter];

                if (lastNote != null && currentNote.StartPosition.GetAsTotalTicks() == lastNote.StartPosition.GetAsTotalTicks() && currentNote.NoteNumber == lastNote.NoteNumber)
                {
                    this.Notes.Remove(currentNote);
                }
                else
                {
                    noteCounter++;
                    lastNote = currentNote;
                }
            }
        }

        private List<Note> _notes;

        public List<Note> Notes
        {
            get { return _notes; }
            set { _notes = value; }
        }

        public override uint GetSizeInBytes()
        {
            uint size = 0;

            if (this.SequenceNumber != null)
                size = size + this.SequenceNumber.GetSizeInBytes();

            if (this.Comments != null)
                size = size + this.Comments.GetSizeInBytes();

            if (this.CopyrightNotice != null)
                size = size + this.CopyrightNotice.GetSizeInBytes();

            if (this.TrackName != null)
                size = size + this.TrackName.GetSizeInBytes();

            if (this.InstrumentName != null)
                size = size + this.InstrumentName.GetSizeInBytes();

            if (this.Lyrics != null)
                size = size + this.Lyrics.GetSizeInBytes();

            if (this.Marker != null)
                size = size + this.Marker.GetSizeInBytes();

            if (this.CuePoint != null)
                size = size + this.CuePoint.GetSizeInBytes();

            if (this.DeviceName != null)
                size = size + this.DeviceName.GetSizeInBytes();

            if (this.Channel != null)
                size = size + this.Channel.GetSizeInBytes();

            if (this.Port != null)
                size = size + this.Port.GetSizeInBytes();

            if (this.Tempo != null)
                size = size + this.Tempo.GetSizeInBytes();

            if (this.SMPTEOffSet != null)
                size = size + this.SMPTEOffSet.GetSizeInBytes();

            if (this.TimeSignature != null)
                size = size + this.TimeSignature.GetSizeInBytes();

            if (this.KeySignature != null)
                size = size + this.KeySignature.GetSizeInBytes();

            foreach (ChannelMidiEvent chan in this.ChannelEvents)
            {
                size = size + chan.GetSizeInBytes();
            }

            size = size + this.EndOfTrack.GetSizeInBytes();

            return size;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            //sb.AppendLine(String.Format("Format Type: {0}", this.))
            return sb.ToString();
        }
    }
}