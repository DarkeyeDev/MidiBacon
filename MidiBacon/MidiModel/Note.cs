using System.Text;

namespace MidiBacon
{
    public class Note
    {
        private MidiTime _startPosition;

        public MidiTime StartPosition
        {
            get { return _startPosition; }
            set { _startPosition = value; }
        }

        private MidiTime _endPosition;

        public MidiTime EndPosition
        {
            get { return _endPosition; }
            set { _endPosition = value; }
        }

        private byte _noteNumber;

        public byte NoteNumber
        {
            get { return _noteNumber; }
            set { _noteNumber = value; }
        }

        private byte _velocity;

        public byte Velocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        }

        private uint _length;

        public uint Length
        {
            get { return _length; }
            set { _length = value; }
        }

        public void CalculateLength()
        {
            this.Length = this.EndPosition.GetAsTotalTicks() - this.StartPosition.GetAsTotalTicks();
        }

        public void CalculateEndPosition()
        {
            this.EndPosition = new MidiTime(this.StartPosition.File, this.StartPosition.GetAsTotalTicks() + this.Length, this.StartPosition.TimeSignature);
        }

        //public MeasuresBeats GetEndPosition()
        //{
        //    return this., this.StartPosition.GetAsTotalTicks()
        //}

        private MidiTime _averagePosition;

        public MidiTime AveragePosition
        {
            get { return _averagePosition; }
            set { _averagePosition = value; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("-------------------------------------------------");
            sb.AppendFormat("Average Position: {0}", this.AveragePosition).AppendLine();
            sb.AppendFormat("Start: {0}", this.StartPosition).AppendLine();
            sb.AppendFormat("End: {0}", this.EndPosition).AppendLine();
            sb.AppendFormat("Length: {0}", this.Length).AppendLine();
            sb.AppendFormat("Note: {0}", this.NoteNumber).AppendLine();
            sb.AppendFormat("Velocity: {0}", this.Velocity).AppendLine();

            return sb.ToString();
        }
    }
}