using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace MidiBacon
{
    public class MidiFile
    {
        public MidiFile()
        {
            this.Header = new HeaderChunk();
            this.Header.File = this;
            this.Tracks = new List<TrackChunk>();
            this.Regions = new List<Region>();
        }

        private String _fileNameAndPath;

        public String FileNameAndPath
        {
            get { return _fileNameAndPath; }
            set { _fileNameAndPath = value; }
        }

        private HeaderChunk _header;

        public HeaderChunk Header
        {
            get { return _header; }
            set { _header = value; }
        }

        private List<TrackChunk> _tracks;

        public List<TrackChunk> Tracks
        {
            get { return _tracks; }
            set { _tracks = value; }
        }

        public void AddTrack(TrackChunk track)
        {
            this.Tracks.Add(track);
            track.File = this;
        }

        public void CreateTracks(int numberOfTracksToCreate)
        {
            for (int i = 0; i < numberOfTracksToCreate; i++)
            {
                this.AddTrack(new TrackChunk());
            }
        }

        private TrackChunk _metaTrack;

        public TrackChunk MetaTrack
        {
            get { return _metaTrack; }
            set { _metaTrack = value; }
        }

        public void Import(String midiFilePathAndFileName)
        {
            this.FileNameAndPath = midiFilePathAndFileName;

            String path = Path.GetDirectoryName(midiFilePathAndFileName);

            String regionDefinitionPathAndFileName = Path.ChangeExtension(midiFilePathAndFileName, "csv");

            if (File.Exists(regionDefinitionPathAndFileName))
            {
                this.LoadRegions(regionDefinitionPathAndFileName);
            }

            using (BinaryReader binaryReader = new BinaryReader(File.Open(midiFilePathAndFileName, FileMode.Open, FileAccess.Read), Encoding.Default))
            {
                if (binaryReader.ReadChar() == 'M')
                    if (binaryReader.ReadChar() == 'T')
                        if (binaryReader.ReadChar() == 'h')
                            if (binaryReader.ReadChar() == 'd')
                            {
                                Debug.WriteLine(this.FileNameAndPath);

                                this.Header = new HeaderChunk();
                                this.Header.File = this;
                                this.Header.ConvertFromBinary(binaryReader);

                                Debug.WriteLine(this.Header);
                                //this.CreateTracks(this.Header.NumberOfTracks);

                                //TrackChunk metaTrack = null;

                                while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
                                {
                                    if (binaryReader.ReadChar() == 'M')
                                        if (binaryReader.ReadChar() == 'T')
                                            if (binaryReader.ReadChar() == 'r')
                                                if (binaryReader.ReadChar() == 'k')
                                                {
                                                    if (this.Header.FormatType == TrackFormatType.MetaAndChannel && this.MetaTrack == null)
                                                    {
                                                        this.MetaTrack = new TrackChunk();
                                                        this.MetaTrack.ConvertFromBinary(binaryReader);
                                                    }
                                                    else
                                                    {
                                                        TrackChunk track = new TrackChunk();
                                                        this.AddTrack(track);
                                                        track.CopyMetaData(this.MetaTrack);
                                                        track.ConvertFromBinary(binaryReader);
                                                        track.Tempo = new TempoEvent(track.Tempo.MicroSecondsPerQuarterNote, track.TimeSignature.Denominator);
                                                        //track.Tempo.BPM = track.Tempo.CalculateBPM(this.Header.PulsesPerQuarterNote, track.TimeSignature.Denominator);//have to calculate at a point when we have time signature and tempo
                                                        track.ConvertEventsToNotes();
                                                        //Debug.WriteLine(track);
                                                    }
                                                }
                                }

                                //if (this.Header.NumberOfTracks != this.Tracks.Count)
                                //    throw new ApplicationException("number of tracks defined in header does not match the number of actual tracks found");
                            }
                            else
                            {
                                throw new ApplicationException("file did not contain expected header");
                            }
            }
        }

        public void Export(String midiFilePathAndFileName)
        {
            using (BinaryWriter binaryWriter = new BinaryWriter(File.Open(midiFilePathAndFileName, FileMode.Create, FileAccess.Write), Encoding.Default))
            {
                this.Header.ConvertToBinary(binaryWriter);

                foreach (TrackChunk track in this.Tracks)
                {
                    //Debug.WriteLine("Number of channel events:" + track.ChannelEvents.Count);
                    //Debug.WriteLine("Size write:" + track.GetSizeInBytes());
                    track.ConvertToBinary(binaryWriter);
                }
            }
        }

        /// <summary>
        /// the position of the region boundaries are calculated on the fly by using the current time
        /// signature and pulses per QN
        /// </summary>
        /// <param name="position"></param>
        /// <param name="pulsesPerQuarterNote"></param>
        /// <param name="timeSig"></param>
        /// <returns></returns>
        public Region GetRegion(MidiTime position, uint pulsesPerQuarterNote, TimeSignatureEvent timeSig)
        {
            uint posTicks = position.GetAsTotalTicks(pulsesPerQuarterNote, timeSig);

            foreach (Region r in this.Regions)
            {
                if (posTicks >= r.Start.GetAsTotalTicks(pulsesPerQuarterNote, timeSig)
                    && posTicks < r.End.GetAsTotalTicks(pulsesPerQuarterNote, timeSig))
                {
                    return r;
                }
            }

            return null;
        }

        public void ConvertToMidiFilesByTrack(String outputPath)
        {
            if (this.Header.FormatType != TrackFormatType.MetaAndChannel)
                throw new ApplicationException("cannot split file");

            List<MidiFile> midiFiles = new List<MidiFile>();

            foreach (TrackChunk origTrack in this.Tracks)
            {
                MidiFile newFile = newFile = new MidiFile();
                newFile.FileNameAndPath = outputPath + Path.DirectorySeparatorChar + origTrack.TrackName.Text + ".mid";
                newFile.Header.NumberOfTracks = 1;
                newFile.Header.FormatType = TrackFormatType.Single;
                newFile.Header.PulsesPerQuarterNote = this.Header.PulsesPerQuarterNote;
                midiFiles.Add(newFile);

                TrackChunk newTrack = new TrackChunk();
                //newTrack.TrackName = new TrackNameEvent(origTrack.TrackName.Text);
                newTrack.TimeSignature = new TimeSignatureEvent(origTrack.TimeSignature.Numerator, origTrack.TimeSignature.Denominator, origTrack.TimeSignature.MetronomePulse, origTrack.TimeSignature.ThirtySecondNotes);
                newTrack.Tempo = new TempoEvent(origTrack.Tempo.MicroSecondsPerQuarterNote, origTrack.TimeSignature.Denominator);
                newTrack.CopyrightNotice = new CopyrightNoticeEvent("Joe Bacon 2013");
                newTrack.DeviceName = new DeviceNameEvent("MIDI-Bacon");
                newFile.Tracks.Add(newTrack);

                foreach (var channelEvent in origTrack.ChannelEvents)
                {
                    newTrack.ChannelEvents.Add(channelEvent);
                }
            }

            foreach (MidiFile mf in midiFiles)
            {
                mf.Export(mf.FileNameAndPath);
            }
        }

        public void ConvertToMidiFilesByRegionAndTrack(String outputPath)
        {
            if (this.Header.FormatType != TrackFormatType.MetaAndChannel)
                throw new ApplicationException("cannot split file");

            List<MidiFile> midiFiles = new List<MidiFile>();

            foreach (TrackChunk origTrack in this.Tracks)
            {
                /// do conversion in this order. by doing these conversions we also remove any
                /// overlapping notes.
                origTrack.MakeAllNotesSameKey(60);  //good for drums where we want the same note for all drum hits. this also means we will clear a lot more duplicate notes.
                origTrack.MakeAllNotesSameLength(240);
                origTrack.RemoveNotesThatDoNotLieCompletelyWithinARegion();
                origTrack.RemoveDuplicateNotes();
                origTrack.CombineOverlappingNotes();
                //origTrack.ApplyScaleToNotesRandomly(5, NoteMap.C, NoteMap.CSharp, NoteMap.F, NoteMap.G, NoteMap.GSharp, NoteMap.A); //C, C#, F, G, G#, A
                //origTrack.ApplyScaleToNotesInSequence(5, 0, true, NoteMap.C, NoteMap.CSharp, NoteMap.F, NoteMap.G, NoteMap.GSharp, NoteMap.A); //C, C#, F, G, G#, A

                TrackChunk newTrack = null;
                MidiFile newFile = null;
                Region currentRegion = null;
                Note lastNote = null;

                foreach (var note in origTrack.Notes)
                {
                    if (note.AveragePosition.Region != currentRegion)
                    {
                        currentRegion = note.StartPosition.Region;

                        newFile = new MidiFile();

                        //String temp = origTrack.TrackName.Text.Substring(0, 2);
                        //if (temp.StartsWith("0"))
                        //    temp = temp.Substring(1);

                        String fileNameNoExt = Path.GetFileNameWithoutExtension(this.FileNameAndPath);

                        String newTrackName = "R" + note.StartPosition.Region.Ordinal.ToString().PadLeft(2, '0') + " - " + origTrack.TrackName.Text + " - " + fileNameNoExt;
                        newFile.FileNameAndPath = outputPath + Path.DirectorySeparatorChar + newTrackName + ".mid";

                        newFile.Header.NumberOfTracks = 1;
                        newFile.Header.FormatType = TrackFormatType.Single;
                        newFile.Header.PulsesPerQuarterNote = this.Header.PulsesPerQuarterNote;
                        midiFiles.Add(newFile);

                        newTrack = new TrackChunk();
                        newTrack.TrackName = new TrackNameEvent(newTrackName);
                        newTrack.TimeSignature = new TimeSignatureEvent(origTrack.TimeSignature.Numerator, origTrack.TimeSignature.Denominator, origTrack.TimeSignature.MetronomePulse, origTrack.TimeSignature.ThirtySecondNotes);
                        newTrack.Tempo = new TempoEvent(origTrack.Tempo.MicroSecondsPerQuarterNote, origTrack.TimeSignature.Denominator);
                        newTrack.CopyrightNotice = new CopyrightNoticeEvent("Joe Bacon 2013");
                        newTrack.DeviceName = new DeviceNameEvent("MIDI-Bacon");
                        newFile.Tracks.Add(newTrack);

                        lastNote = null;
                    }

                    uint deltaTime = 0;

                    if (lastNote == null)
                        deltaTime = note.StartPosition.GetAsTotalTicks() - currentRegion.Start.GetAsTotalTicks(this.Header.PulsesPerQuarterNote, origTrack.TimeSignature);  //occurs on first note of measure
                    else
                    {
                        deltaTime = note.StartPosition.GetAsTotalTicks() - lastNote.EndPosition.GetAsTotalTicks();
                    }

                    ChannelMidiEvent noteOnEvent = ChannelMidiEvent.CreateNoteOnChannelMidiEvent(0, note.NoteNumber, note.Velocity, deltaTime);
                    newTrack.ChannelEvents.Add(noteOnEvent);

                    ChannelMidiEvent noteOffEvent = ChannelMidiEvent.CreateNoteOffChannelMidiEvent(0, note.NoteNumber, note.Length);
                    newTrack.ChannelEvents.Add(noteOffEvent);

                    lastNote = note;
                }
            }

            foreach (MidiFile mf in midiFiles)
            {
                mf.Export(mf.FileNameAndPath);
            }
        }

        public void ConvertToMidiFilesByRegionAndBarAndTrack(String outputPath)
        {
            if (this.Header.FormatType != TrackFormatType.MetaAndChannel)
                throw new ApplicationException("cannot split file");

            List<MidiFile> midiFiles = new List<MidiFile>();

            foreach (TrackChunk origTrack in this.Tracks)
            {
                /// do conversion in this order. by doing these conversions we also remove any
                /// overlapping notes.
                origTrack.MakeAllNotesSameKey(48);  //good for drums where we want the same note for all drum hits
                origTrack.MakeAllNotesSameLength(10);    //smallest unit that wont overlap after cleaning duplicates
                //origTrack.ConstrainNotesToMeasureBoundaries();  //makes it easy to put the notes into 'measure' files
                //origTrack.RemoveNotesNotInMeasure();
                origTrack.RemoveNotesWhereNoteStartIsNotWithinMeasure();
                //origTrack.RemoveDuplicateNotes();
                origTrack.RemoveDuplicateNotes();

                //uint currentMeasure = 0;
                uint currentRegionMeasure = 0;

                TrackChunk newTrack = null;
                MidiFile newFile = null;
                Region currentRegion = null;
                MidiTime currentMeasureStart = null;
                Note lastNote = null;

                foreach (var note in origTrack.Notes)
                {
                    if (note.StartPosition.Region == null)
                    {
                        Debug.WriteLine(origTrack.TrackName.Text + " null region @" + note.StartPosition);

                        //Debug.WriteLine(note.ToString());
                        continue;
                    }

                    if (note.StartPosition.Region != currentRegion)
                    {
                        ///the first note in a new region
                        ///
                        currentRegion = note.StartPosition.Region;
                        currentRegionMeasure = 0;   //reset
                    }

                    if (currentMeasureStart == null || note.StartPosition.Measures != currentMeasureStart.Measures)
                    {
                        currentRegionMeasure++;

                        ///the first note in a new measure
                        ///
                        newFile = new MidiFile();

                        String temp = origTrack.TrackName.Text.Substring(0, 2);
                        //if (temp.StartsWith("0"))
                        //    temp = temp.Substring(1);

                        String newTrackName = note.StartPosition.Region.ID + "-B" + currentRegionMeasure + "-T" + temp;
                        newFile.FileNameAndPath = outputPath + Path.DirectorySeparatorChar + newTrackName + ".mid";

                        newFile.Header.NumberOfTracks = 1;
                        newFile.Header.FormatType = TrackFormatType.Single;
                        newFile.Header.PulsesPerQuarterNote = this.Header.PulsesPerQuarterNote;
                        midiFiles.Add(newFile);

                        newTrack = new TrackChunk();
                        newTrack.TrackName = new TrackNameEvent(newTrackName);
                        newTrack.TimeSignature = new TimeSignatureEvent(origTrack.TimeSignature.Numerator, origTrack.TimeSignature.Denominator, origTrack.TimeSignature.MetronomePulse, origTrack.TimeSignature.ThirtySecondNotes);
                        newTrack.Tempo = new TempoEvent(origTrack.Tempo.MicroSecondsPerQuarterNote, origTrack.TimeSignature.Denominator);
                        newTrack.CopyrightNotice = new CopyrightNoticeEvent("Joe Bacon 2013");
                        newTrack.DeviceName = new DeviceNameEvent("MIDI-Bacon");
                        newFile.Tracks.Add(newTrack);

                        currentMeasureStart = new MidiTime(this, note.StartPosition.Measures, 1, 0, origTrack.TimeSignature);
                        lastNote = null;
                    }

                    uint deltaTime = 0;

                    if (lastNote == null)
                        deltaTime = note.StartPosition.GetAsTotalTicks() - currentMeasureStart.GetAsTotalTicks();  //occurs on first note of measure
                    else
                    {
                        deltaTime = note.StartPosition.GetAsTotalTicks() - lastNote.EndPosition.GetAsTotalTicks();
                    }

                    ChannelMidiEvent noteOnEvent = ChannelMidiEvent.CreateNoteOnChannelMidiEvent(0, note.NoteNumber, note.Velocity, deltaTime);
                    newTrack.ChannelEvents.Add(noteOnEvent);

                    ChannelMidiEvent noteOffEvent = ChannelMidiEvent.CreateNoteOffChannelMidiEvent(0, note.NoteNumber, note.Length);
                    newTrack.ChannelEvents.Add(noteOffEvent);

                    lastNote = note;
                }
            }

            foreach (MidiFile mf in midiFiles)
            {
                mf.Export(mf.FileNameAndPath);
            }
        }

        private List<Region> _regions;

        public List<Region> Regions
        {
            get { return _regions; }
            set { _regions = value; }
        }

        public void LoadRegions(String regionDefinitionsPathAndFileName)
        {
            using (TextReader reader = new StreamReader(regionDefinitionsPathAndFileName, Encoding.ASCII))
            {
                String line;

                bool headerRow = true;

                while ((line = reader.ReadLine()) != null)
                {
                    if (headerRow)
                    {
                        headerRow = false;
                        continue;
                    }

                    char[] splitChars = { ',' };
                    String[] parts = line.Split(splitChars);

                    Region r = new Region();
                    r.Ordinal = this.Regions.Count + 1;
                    r.ID = parts[0];
                    r.Name = parts[1];
                    r.Start = new MidiTime(this, parts[2], null);
                    r.End = new MidiTime(this, parts[3], null);
                    r.Length = new MidiTime(this, parts[4], null);

                    this.Regions.Add(r);
                }
            }
        }
    }
}