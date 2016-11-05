using System;

namespace MidiBacon
{
    public class Region
    {
        private int _ordinal;

        public int Ordinal
        {
            get { return _ordinal; }
            set { _ordinal = value; }
        }

        private String _id;

        public String ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private String _name;

        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private MidiTime _start;

        public MidiTime Start
        {
            get { return _start; }
            set { _start = value; }
        }

        private MidiTime _end;

        public MidiTime End
        {
            get { return _end; }
            set { _end = value; }
        }

        private MidiTime _length;

        public MidiTime Length
        {
            get { return _length; }
            set { _length = value; }
        }

        public override string ToString()
        {
            return String.Format("{0},{1},{2},{3},{4}", this.ID, this.Name, this.Start, this.End, this.Length);
        }
    }
}