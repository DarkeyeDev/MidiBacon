using System;

namespace MidiBacon
{
    /// <summary>
    /// This meta event is used to set a sequences time signature. The time signature defined with 4
    /// bytes, a numerator, a denominator, a metronome pulse and number of 32nd notes per MIDI
    /// quarter-note. The numerator is specified as a literal value, but the denominator is specified
    /// as (get ready) the value to which the power of 2 must be raised to equal the number of
    /// subdivisions per whole note. For example, a value of 0 means a whole note because 2 to the
    /// power of 0 is 1 (whole note), a value of 1 means a half-note because 2 to the power of 1 is 2
    /// (half-note), and so on. The metronome pulse specifies how often the metronome should click in
    /// terms of the number of clock signals per click, which come at a rate of 24 per quarter-note.
    /// For example, a value of 24 would mean to click once every quarter-note (beat) and a value of
    /// 48 would mean to click once every half-note (2 beats). And finally, the fourth byte specifies
    /// the number of 32nd notes per 24 MIDI clock signals. This value is usually 8 because there are
    /// usually 8 32nd notes in a quarter-note. At least one Time Signature Event should appear in
    /// the first track chunk (or all track chunks in a Type 2 file) before any non-zero delta time
    /// events. If one is not specified 4/4, 24, 8 should be assumed.
    ///
    /// 6/8 = 6 eighth notes per bar
    /// </summary>
    public class TimeSignatureEvent : BaseMetaEvent
    {
        public TimeSignatureEvent(byte numerator = 4, byte denominator = 4, byte metronome = 24, byte thirtySecondNotes = 8)
        {
            this.Type = MetaEventType.TimeSignature;
            this.Numerator = numerator;
            this.Denominator = denominator;
            this.MetronomePulse = metronome;
            this.ThirtySecondNotes = thirtySecondNotes;
        }

        public override uint DataLength
        {
            get
            {
                return 4;
            }
        }

        private byte _numerator;

        /// <summary>
        /// The numerator represents the numerator of the time signature that you would find on
        /// traditional sheet music. The numerator counts the number of beats in a measure. For
        /// example a numerator of 4 means that each bar contains four beats. This is important to
        /// know because usually the first beat of each bar has extra emphasis.
        /// </summary>
        public byte Numerator
        {
            get { return _numerator; }
            set { _numerator = value; }
        }

        private byte _denominator;

        /// <summary>
        /// The denominator represents the denominator of the time signature that you would find on
        /// traditional sheet music. The denominator specifies the number of quarter notes in a beat.
        /// A time signature of 4,4 means: 4 beats in the bar and each beat is a quarter note (i.e. a
        /// crotchet). In MIDI the denominator value is stored in a special format. i.e. the real
        /// denominator = 2^[dd].
        /// </summary>
        public byte Denominator
        {
            get { return _denominator; }
            set { _denominator = value; }
        }

        //private byte _rawDenominator;

        //public byte RawDenominator
        //{
        //    get { return _rawDenominator; }
        //    set { _rawDenominator = value; }
        //}

        public byte CalculateRawDenominator()
        {
            byte numberOfTimesWeCanDivideByTwo = 1;

            byte result = this.Denominator;

            while (result != 2)
            {
                result = (byte)(result / 2);
                numberOfTimesWeCanDivideByTwo++;
            }

            return numberOfTimesWeCanDivideByTwo;
        }

        private byte _metronome;

        /// <summary>
        /// The standard MIDI clock ticks every 24 times every quarter note (crotchet) so a [cc]
        /// value of 24 would mean that the metronome clicks once every quarter note. A [cc] value of
        /// 6 would mean that the metronome clicks once every 1/8th of a note (quaver). Be warned,
        /// this midi clock is different from the clock who's pulses determine the start time and
        /// duration of the notes (see PPQN below). This MIDI clock ticks 24 times a second and seems
        /// to be used only to specify the rate of the metronome - which I can only assume is a real
        /// metronome i.e. a device which makes a tick noise at a steady rate... tick, tick tick...
        /// </summary>
        public byte MetronomePulse
        {
            get { return _metronome; }
            set { _metronome = value; }
        }

        private byte _thirtySecondNotes;

        /// <summary>
        /// This value specifies the number of 1/32nds of a note happen every MIDI quarter note. It
        /// is usually 8 which means that a quarter note happens every quarter note - which is
        /// logical. By choosing different values it's possible to vary the rate of the music
        /// artificially. By putting a value of 16 it means that the music plays two quarter notes
        /// for each quarter note metered out by the midi clock. This means the music plays at double speed.
        /// </summary>
        public byte ThirtySecondNotes
        {
            get { return _thirtySecondNotes; }
            set { _thirtySecondNotes = value; }
        }

        public override void ConvertToBinary(System.IO.BinaryWriter writer)
        {
            if (this.DataLength > 0)
            {
                base.ConvertToBinary(writer);
                UtilBinary.WriteValue(writer, this.Numerator);
                UtilBinary.WriteValue(writer, this.CalculateRawDenominator());
                UtilBinary.WriteValue(writer, this.MetronomePulse);
                UtilBinary.WriteValue(writer, this.ThirtySecondNotes);
            }
        }

        public override string ToString()
        {
            return String.Format("Time Signiture: {0}/{1} {2}Metr. {3}N/q", this.Numerator, this.Denominator, this.MetronomePulse, this.ThirtySecondNotes);
        }
    }
}