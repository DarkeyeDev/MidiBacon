using System;

namespace MidiBacon
{
    /// <summary>
    /// This meta event defines the name of a sequence when in a Type 0 or Type 2 MIDI file or in the
    /// first track of a Type 1 MIDI file. It defines a track name when it appears in any track after
    /// the first in a Type 1 MIDI file. This meta event should always have a delta time of 0 and
    /// come before all MIDI Channel Events and non-zero delta time events.
    /// </summary>
    public class TrackNameEvent : BaseMetaEvent
    {
        public TrackNameEvent(String text)
        {
            this.Type = MetaEventType.TrackName;
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