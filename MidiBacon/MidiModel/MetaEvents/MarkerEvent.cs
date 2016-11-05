using System;

namespace MidiBacon
{
    /// <summary>
    /// This meta event marks a significant point in time for the sequence. It is usually found in
    /// the first track chunk, but may appear in any one. This event can be useful for marking the
    /// beginning/end of a new verse or chorus.
    /// </summary>
    public class MarkerEvent : BaseMetaEvent
    {
        public MarkerEvent(String text)
        {
            this.Type = MetaEventType.Marker;
            this.Text = text;
        }

        private String _text;

        public String Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public override uint DataLength
        {
            get
            {
                return (uint)this.Text.Length;
            }
        }

        public override void ConvertToBinary(System.IO.BinaryWriter writer)
        {
            if (this.DataLength > 0)
            {
                base.ConvertToBinary(writer);
                UtilBinary.WriteValue(writer, this.Text.ToCharArray());
            }
        }
    }
}