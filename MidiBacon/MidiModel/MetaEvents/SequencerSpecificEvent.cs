//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace MidiBacon
//{
//    /// <summary>
//    /// This meta event is used to specify information specific to a hardware or software sequencer. The first Data byte (or three bytes if the first byte is 0) specifies the manufacturer's ID and the following bytes contain information specified by the manufacturer. The individual manufacturers may document this information in their respective manuals.
//    /// </summary>
//    public class SequencerSpecificEvent : BaseMetaEvent
//    {
//        public SequencerSpecificEvent()
//        {
//            this.Type = MetaEventType.SequencerSpecific;
//        }

// public override uint DataLength { get { throw new NotImplementedException(); } }

// private object _data;

// public object Data { get { return _data; } set { _data = value; } }

//        public override void ConvertToBinary(System.IO.BinaryWriter writer)
//        {
//            if (this.DataLength > 0)
//            {
//                base.ConvertToBinary(writer);
//                //writer.Write(this.Data);
//            }
//        }
//    }
//}