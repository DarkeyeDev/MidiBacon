using System;
using System.Text;

namespace MidiBacon
{
    public class HeaderChunk : BaseChunk
    {
        public HeaderChunk()
            : base()
        {
            this.ChunkID = new char[4] { 'M', 'T', 'h', 'd' };
        }

        public HeaderChunk(TrackFormatType formatType, ushort numberOfTracks, ushort timeDivision)
            : this()
        {
            this.FormatType = formatType;
            this.NumberOfTracks = numberOfTracks;
            this.PulsesPerQuarterNote = timeDivision;
        }

        private TrackFormatType _formatType;

        public TrackFormatType FormatType
        {
            get { return _formatType; }
            set { _formatType = value; }
        }

        private ushort _numberOfTracks;

        public ushort NumberOfTracks
        {
            get { return _numberOfTracks; }
            set { _numberOfTracks = value; }
        }

        private ushort _pulsePerQuarterNote;

        /// <summary>
        /// how many Pulses (i.e. clocks or ticks) Per Quarter Note (abbreviated as PPQN) resolution
        /// the time-stamps are based upon, A.K.A Division or resolution
        /// </summary>
        public ushort PulsesPerQuarterNote
        {
            get { return _pulsePerQuarterNote; }
            set { _pulsePerQuarterNote = value; }
        }

        public void ConvertToBinary(System.IO.BinaryWriter writer)
        {
            UtilBinary.WriteValue(writer, this.ChunkID);
            UtilBinary.WriteValue(writer, this.GetSizeInBytes());
            UtilBinary.WriteValue(writer, (ushort)this.FormatType);
            UtilBinary.WriteValue(writer, this.NumberOfTracks);
            UtilBinary.WriteValue(writer, this.PulsesPerQuarterNote);
        }

        public void ConvertFromBinary(System.IO.BinaryReader reader)
        {
            //this.ChunkID = UtilBinary.ReadCharsValue(reader, 4);  ///read by MidiFile
            this.ChunkSize = UtilBinary.ReadUintValue(reader);
            this.FormatType = (TrackFormatType)UtilBinary.ReadUshortValue(reader);
            this.NumberOfTracks = UtilBinary.ReadUshortValue(reader);
            this.PulsesPerQuarterNote = UtilBinary.ReadUshortValue(reader);
        }

        /// <summary>
        /// does not include the chunkID or the size of the size itself. only the FormatType,
        /// NumberOfTracks, TimeDivision
        /// </summary>
        /// <returns></returns>
        public override uint GetSizeInBytes()
        {
            return sizeof(TrackFormatType) + sizeof(ushort) + sizeof(ushort);    //should always return 6 bytes
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(String.Format("Format Type: {0}", this.FormatType));
            sb.AppendLine(String.Format("NumberOfTracks: {0}", this.NumberOfTracks));
            sb.AppendLine(String.Format("PulsesPerQuarterNote: {0}", this.PulsesPerQuarterNote));
            return sb.ToString();
        }
    }
}