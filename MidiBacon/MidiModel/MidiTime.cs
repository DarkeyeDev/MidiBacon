using System;

namespace MidiBacon
{
    public class MidiTime
    {
        public MidiTime()
        {
        }

        private TimeSignatureEvent _timeSignature;

        public TimeSignatureEvent TimeSignature
        {
            get { return _timeSignature; }
            set { _timeSignature = value; }
        }

        public MidiTime(MidiTime copyMe)
        {
            this.File = copyMe.File;
            this.TimeSignature = copyMe.TimeSignature;
            this.Measures = copyMe.Measures;
            this.Beats = copyMe.Beats;
            this.Ticks = copyMe.Ticks;
            this.Region = copyMe.Region;
        }

        public MidiTime(MidiFile midiFile, uint measures, uint beats, uint ticks, TimeSignatureEvent timeSig)
        {
            this.File = midiFile;
            this.TimeSignature = timeSig;

            this.Measures = measures;

            if (this.Measures < 1)
                throw new ApplicationException("measures must be 1 or above");

            this.Beats = beats;
            this.Ticks = ticks;

            //uint convertedPulsesPerQuarterNote = (uint)(this.File.Header.PulsesPerQuarterNote / (this.TimeSignature.Denominator / 4));

            this.Region = this.File.GetRegion(this, this.File.Header.PulsesPerQuarterNote, this.TimeSignature);
        }

        public MidiTime(MidiFile midiFile, String measuresBeatsTicksString, TimeSignatureEvent timeSig)
        {
            this.File = midiFile;
            this.TimeSignature = timeSig;

            char[] splitChars = { '.' };
            String[] parts = measuresBeatsTicksString.Split(splitChars);
            this.Measures = uint.Parse(parts[0]);

            if (this.Measures < 1)
                throw new ApplicationException("measures must be 1 or above");

            this.Beats = uint.Parse(parts[1]);
            this.Ticks = uint.Parse(parts[2]);

            //this.Region = this.File.GetRegion(this, this.File.Header.PulsesPerQuarterNote, this.TimeSignature);
        }

        public MidiTime(MidiFile midiFile, uint totalTicks, TimeSignatureEvent timeSig)
        {
            this.File = midiFile;
            this.TimeSignature = timeSig;

            uint convertedPulsesPerQuarterNote = (uint)(this.File.Header.PulsesPerQuarterNote / (this.TimeSignature.Denominator / 4));

            this.Measures = ((totalTicks / convertedPulsesPerQuarterNote) / this.TimeSignature.Numerator); //number of beats in a measure
            this.Beats = (totalTicks / convertedPulsesPerQuarterNote) % this.TimeSignature.Numerator;    //number of notes in a beat
            this.Ticks = (totalTicks % convertedPulsesPerQuarterNote);

            this.Measures++;
            this.Beats++;

            this.Region = this.File.GetRegion(this, this.File.Header.PulsesPerQuarterNote, this.TimeSignature);
        }

        public uint GetAsTotalTicks(uint pulsesPerQuarterNote, TimeSignatureEvent timeSig)
        {
            uint ticks = 0;

            uint convertedPulsesPerQuarterNote = (uint)(pulsesPerQuarterNote / (timeSig.Denominator / 4));

            ticks += ((this.Measures - 1) * convertedPulsesPerQuarterNote * timeSig.Numerator);
            ticks += ((this.Beats - 1) * convertedPulsesPerQuarterNote);
            ticks += this.Ticks;

            return ticks;
        }

        public uint GetAsTotalTicks()
        {
            if (this.TimeSignature == null)
                throw new ArgumentNullException("TimeSignature", "if this is a midiTime for a REGION start/end/etc then it does not have a timesignature. use the overload of this method");

            uint ticks = 0;

            uint convertedPulsesPerQuarterNote = (uint)(this.File.Header.PulsesPerQuarterNote / (this.TimeSignature.Denominator / 4));
            //uint convertedPulsesPerQuarterNote = this.File.Header.PulsesPerQuarterNote;

            ticks += ((this.Measures - 1) * convertedPulsesPerQuarterNote * this.TimeSignature.Numerator);
            ticks += ((this.Beats - 1) * convertedPulsesPerQuarterNote);
            ticks += this.Ticks;

            return ticks;
        }

        private MidiFile _file;

        public MidiFile File
        {
            get { return _file; }
            set { _file = value; }
        }

        private Region _region;

        public Region Region
        {
            get { return _region; }
            set { _region = value; }
        }

        private uint _measures;

        /// <summary>
        /// aka Bars. 1- based index
        /// </summary>
        public uint Measures
        {
            get { return _measures; }
            set { _measures = value; }
        }

        private uint _beats;

        public uint Beats
        {
            get { return _beats; }
            set { _beats = value; }
        }

        private uint _ticks;

        public uint Ticks
        {
            get { return _ticks; }
            set { _ticks = value; }
        }

        public override string ToString()
        {
            return String.Format("{0}.{1}.{2}", this.Measures, this.Beats, this.Ticks);
        }
    }
}