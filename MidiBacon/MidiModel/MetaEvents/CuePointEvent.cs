using System;

namespace MidiBacon
{
    /// <summary>
    /// This meta event marks the start of some type of new sound or action. It is usually found in
    /// the first track chunk, but may appear in any one. This event is sometimes used by sequencers
    /// to mark when playback of a sample or video should begin.
    /// </summary>
    public class CuePointEvent : BaseMetaEvent
    {
        public CuePointEvent(String text)
        {
            this.Type = MetaEventType.CuePoint;
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