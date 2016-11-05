using System;

namespace MidiBacon
{
    /// <summary>
    /// This meta event sets the sequence tempo in terms of microseconds per quarter-note which is
    /// encoded in three bytes. It usually is found in the first track chunk, time-aligned to occur
    /// at the same time as a MIDI clock message to promote more accurate synchronization. If no set
    /// tempo event is present, 120 beats per minute is assumed. The following formula's can be used
    /// to translate the tempo from microseconds per quarter-note to beats per minute and back.
    /// </summary>
    public class TempoEvent : BaseMetaEvent
    {
        public TempoEvent(double bpm = 120.0)
        {
            this.Type = MetaEventType.Tempo;
            this.BPM = bpm;
            this.MicroSecondsPerQuarterNote = this.CalculateMicroSecondsPerQuarterNote(bpm);
        }

        public TempoEvent(uint microSecondsPerQuarterNote, uint timeSignatureDenominator = 4)
        {
            this.Type = MetaEventType.Tempo;
            this.MicroSecondsPerQuarterNote = microSecondsPerQuarterNote;
            this.BPM = this.CalculateBPM(microSecondsPerQuarterNote, timeSignatureDenominator);
        }

        private double _bpm;

        public double BPM
        {
            get { return _bpm; }
            set { _bpm = value; }
        }

        private uint _microSecondsPerQuarterNote;

        public uint MicroSecondsPerQuarterNote
        {
            get { return _microSecondsPerQuarterNote; }
            set { _microSecondsPerQuarterNote = value; }
        }

        public const uint MICROSECONDS_PER_MINUTE = 60000000;

        public override uint DataLength
        {
            get
            {
                return 3;
            }
        }

        public uint CalculateMicroSecondsPerQuarterNote(double bpm)
        {
            return (uint)Math.Round((double)MICROSECONDS_PER_MINUTE / bpm);
        }

        public double CalculateBPM(uint microSecondsPerQuarterNote, uint timeSignatureDenominator)
        {
            return Math.Round(((double)MICROSECONDS_PER_MINUTE / (double)microSecondsPerQuarterNote) * (timeSignatureDenominator / 4.0f));
        }

        public override void ConvertToBinary(System.IO.BinaryWriter writer)
        {
            if (this.DataLength > 0)
            {
                base.ConvertToBinary(writer);
                UtilBinary.WriteValue(writer, this.MicroSecondsPerQuarterNote, 3);
            }
        }

        //const float kOneMinuteInMicroseconds = 60000000;
        //const float kTimeSignatureNumerator = 4.0f; // For show only, use the actual time signature numerator
        //const float kTimeSignatureDenominator = 4.0f; // For show only, use the actual time signature denominator
        //// This is correct
        //float BPM = (kOneMinuteInMicroseconds / newMicrosecondsPerQuarterNote) * (kTimeSignatureDenominator / 4.0f);
    }
}