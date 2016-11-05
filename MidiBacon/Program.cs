using System;
using System.IO;

namespace MidiBacon
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //foreach (String s in Directory.GetFiles(@"C:\Projects\Midi\Test files\multi taiko tracks\", "*.mid", SearchOption.TopDirectoryOnly))
            //{
            //    //String output = String.Format(@"C:\Projects\Midi\Test files\output\output{0}.mid", DateTime.Now.Ticks);
            //    String output = String.Format(@"C:\Projects\Midi\Test files\output\");

            // if (Directory.Exists(output)) { Directory.Delete(output, true);
            // System.Threading.Thread.Sleep(1000); }

            // Directory.CreateDirectory(output); System.Threading.Thread.Sleep(1000);

            //    //ReadFileTest(s);
            //    //ReadWriteFileTest(s, output);
            //    //CreateFileTest(output);
            //    ReadSplitWriteFileTest(s, output);
            //}

            //String output = String.Format(@"C:\Projects\Midi\Test files\output\output{0}.mid", DateTime.Now.Ticks);
            //ReadWriteFileTest(@"C:\Projects\Midi\Test files\TEST INPUT 3.mid", output);

            foreach (String midiFilePathAndNameIN in Directory.GetFiles(@"C:\MUSIC\Samples\Sample Packs\TAIKO\Performance Multis\", "*.mid", SearchOption.TopDirectoryOnly))
            {
                String fileName = Path.GetFileNameWithoutExtension(midiFilePathAndNameIN);

                String midiPathOUT = String.Format(@"C:\MUSIC\Samples\Sample Packs\TAIKO\MidiRenders\{0}\", fileName);
                String wavPath = String.Format(@"C:\MUSIC\Samples\Sample Packs\TAIKO\WavRenders\{0}\", fileName);

                if (Directory.Exists(midiPathOUT))
                {
                    Directory.Delete(midiPathOUT, true);
                    System.Threading.Thread.Sleep(1000);
                }

                Directory.CreateDirectory(midiPathOUT);
                System.Threading.Thread.Sleep(1000);
                ReadSplitWriteFileTest(midiFilePathAndNameIN, midiPathOUT);//turns 1 midi file into many files

                DeleteWavFilesWithNoMidiFile(midiPathOUT, wavPath);
            }
        }

        private static void ReadFileTest(String pathAndFilename)
        {
            MidiFile mf = new MidiFile();
            mf.Import(pathAndFilename);
        }

        private static void ReadWriteFileTest(String input, String output)
        {
            MidiFile mf = new MidiFile();
            mf.Import(input);
            mf.Export(output);
        }

        private static void ReadSplitWriteFileTest(String input, String output)
        {
            MidiFile midi = new MidiFile();
            midi.Import(input);
            midi.ConvertToMidiFilesByTrack(output);
            midi.ConvertToMidiFilesByRegionAndTrack(output);
        }

        private static void DeleteWavFilesWithNoMidiFile(String midiPath, String wavPath)
        {
            foreach (String wavFile in Directory.GetFiles(wavPath, "*.wav", SearchOption.AllDirectories))
            {
                String midiFile = midiPath + Path.GetFileNameWithoutExtension(wavFile) + ".mid";

                if (!File.Exists(midiFile))
                {
                    System.Diagnostics.Debug.WriteLine("Deleting wav file with no corresponding midi file: " + wavFile);
                    File.Delete(wavFile);
                }
            }
        }

        private static void CreateFileTest(String pathAndFilename)
        {
            MidiFile myFile = new MidiFile();
            myFile.Header.FormatType = TrackFormatType.Single;
            myFile.Header.NumberOfTracks = 1;
            myFile.Header.PulsesPerQuarterNote = 960;

            TrackChunk tc1 = new TrackChunk();
            tc1.File = myFile;
            myFile.Tracks.Add(tc1);

            tc1.File = myFile;

            tc1.SequenceNumber = new SequenceNumberEvent(0, 0);
            tc1.Comments = new TextEvent("Comments");
            tc1.CopyrightNotice = new CopyrightNoticeEvent("CopyrightNotice");
            tc1.TrackName = new TrackNameEvent("TrackName");
            tc1.InstrumentName = new InstrumentNameEvent("InstrumentName");
            tc1.Lyrics = new LyricsEvent("Lyrics");
            tc1.Marker = new MarkerEvent("Marker");
            tc1.CuePoint = new CuePointEvent("CuePoint");
            tc1.DeviceName = new DeviceNameEvent("DeviceName");
            tc1.Channel = new MidiChannelEvent(3);
            tc1.Port = new MidiPortEvent(6);
            tc1.TimeSignature = new TimeSignatureEvent(4, 4, 24, 8);
            tc1.Tempo = new TempoEvent(130.0);
            //tc1.SMPTEOffSet = new SMPTEOffSetEvent();
            tc1.KeySignature = new KeySignatureEvent(0, 1);

            tc1.ChannelEvents.Add(ChannelMidiEvent.CreateNoteOnChannelMidiEvent(0, 64, 100, 0));
            tc1.ChannelEvents.Add(ChannelMidiEvent.CreateNoteOffChannelMidiEvent(0, 64, 960));
            tc1.ChannelEvents.Add(ChannelMidiEvent.CreateNoteOnChannelMidiEvent(0, 63, 90, 0));
            tc1.ChannelEvents.Add(ChannelMidiEvent.CreateNoteOffChannelMidiEvent(0, 63, 960));

            myFile.Export(pathAndFilename);
        }
    }
}