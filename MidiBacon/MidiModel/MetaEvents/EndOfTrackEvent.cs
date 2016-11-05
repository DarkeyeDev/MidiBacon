namespace MidiBacon
{
    /// <summary>
    /// This meta event is used to signal the end of a track chunk and must always appear as the last
    /// event in every track chunk.
    /// </summary>
    public class EndOfTrackEvent : BaseMetaEvent
    {
        public EndOfTrackEvent()
        {
            this.Type = MetaEventType.EndOfTrack;
        }

        public override uint DataLength
        {
            get
            {
                return 0;
            }
        }

        public override uint GetSizeInBytes()
        {
            return UtilBinary.GetLengthOfVariableLengthValue(this.DeltaTicks)    //base holds the delta ticks should always be zero for meta event
                + sizeof(byte)  //meta event id FF
                + sizeof(byte) //meta type
                + UtilBinary.GetLengthOfVariableLengthValue(this.DataLength)    //the length of the number that stores the length of the data
                + (uint)this.DataLength;
        }

        public override string ToString()
        {
            return "EndOfTrack";
        }
    }
}