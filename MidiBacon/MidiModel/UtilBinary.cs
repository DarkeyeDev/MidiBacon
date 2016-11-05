using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace MidiBacon
{
    /// <summary>
    /// Most significant bit - always left most bit. does not change with endianess
    /// Most significant BYTE - changes with endianess.
    /// Left = big endian and right = little endian
    /// </summary>
    public abstract class UtilBinary
    {
        public static uint ReadUintValue(BinaryReader reader)
        {
            Byte[] bytes = BitConverter.GetBytes(reader.ReadUInt32());

            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);

            return BitConverter.ToUInt32(bytes, 0);
        }

        public static uint Read3ByteInteger(BinaryReader reader)
        {
            byte[] bytes = reader.ReadBytes(3);

            //ShowBits2(bytes);

            //if (BitConverter.IsLittleEndian)
            //    Array.Reverse(bytes);

            //reverse to little endian. use custom method of doing this because it is an uncommon data type e.g. 3 bytes
            int value = (bytes[2] << 0) | (bytes[1] << 8) | (bytes[0] << 16);

            return (uint)value;
        }

        public static ushort ReadUshortValue(BinaryReader reader)
        {
            Byte[] bytes = BitConverter.GetBytes(reader.ReadUInt16());

            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);

            return BitConverter.ToUInt16(bytes, 0);
        }

        public static byte ReadByteValue(BinaryReader reader)
        {
            return reader.ReadByte();
        }

        public static char[] ReadCharsValue(BinaryReader reader, int count)
        {
            return reader.ReadChars(count);
        }

        public static void WriteValue(BinaryWriter writer, uint value, int count = 4)
        {
            Byte[] bytes = BitConverter.GetBytes(value);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);

            writer.Write(bytes, 4 - count, count);
        }

        public static void WriteValue(BinaryWriter writer, ushort value)
        {
            Byte[] bytes = BitConverter.GetBytes(value);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);

            writer.Write(bytes);
        }

        public static void WriteValue(BinaryWriter writer, byte value)
        {
            writer.Write(value);
        }

        public static void WriteValue(BinaryWriter writer, char[] value)
        {
            writer.Write(value);
        }

        private static void ShowBitsBrokenInToBytes(byte[] bytes, bool MsbToLsb = true)
        {
            BitArray bits = new BitArray(bytes);

            StringBuilder sb = new StringBuilder();

            if (MsbToLsb)
            {
                int byteIndex = bytes.Length - 1;

                sb.AppendLine("MSB--LSB");

                //sb.Append(String.Empty.PadLeft(32 - bits.Length, ' '));

                for (int counter = bits.Length - 1; counter >= 0; counter--)
                {
                    sb.Append(bits[counter] ? "1" : "0");

                    if ((counter) % 8 == 0)
                    {
                        sb.Append(" = " + bytes[byteIndex]);
                        byteIndex--;
                        sb.AppendLine("");
                    }
                }
            }
            else
            {
                int byteIndex = 0;

                sb.AppendLine("LSB--MSB");

                //sb.Append(String.Empty.PadLeft(32 - (bits.Length + 1), ' '));

                for (int counter = 0; counter < bits.Length; counter++)
                {
                    sb.Append(bits[counter] ? "1" : "0");

                    if ((counter + 1) % 8 == 0)
                    {
                        sb.Append(" = " + bytes[byteIndex]);
                        byteIndex++;
                        sb.AppendLine("");
                    }
                }
            }

            sb.AppendLine("");

            Debug.Write(sb.ToString());
        }

        private static void ShowBitsOnSingleLine(byte[] bytes, bool MsbToLsb = true)
        {
            BitArray bits = new BitArray(bytes);
            StringBuilder sb = new StringBuilder();

            if (MsbToLsb)
            {
                sb.AppendLine("MSB-----|-------|-------|----LSB");

                sb.Append(String.Empty.PadLeft(32 - bits.Length, ' '));

                for (int counter = bits.Length - 1; counter >= 0; counter--)
                {
                    sb.Append(bits[counter] ? "1" : "0");
                }
            }
            else
            {
                sb.AppendLine("LSB----|-------|-------|----MSB");

                //sb.Append(String.Empty.PadLeft(32 - (bits.Length + 1), ' '));

                for (int counter = 0; counter < bits.Length; counter++)
                {
                    sb.Append(bits[counter] ? "1" : "0");
                }
            }

            sb.AppendLine("");

            Debug.Write(sb.ToString());
        }

        public static byte CombineBytes(byte msb, byte lsb)
        {
            byte value = (byte)((msb << 4) | ((lsb << 4) >> 4));
            return value;
        }

        public static byte[] SplitByte(byte b)
        {
            byte[] parts = new byte[2];
            parts[0] = (byte)(b >> 4); //shift higher half into lower half
            parts[1] = (byte)((byte)(b << 4) >> 4); //shift higher half outside, shift back
            return parts;
        }

        /// <summary>
        /// returns 1, 2, 3 or 4 (bytes)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static uint GetLengthOfVariableLengthValue(uint value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException("value", value, "value must be 0 or greater");

            uint length = 1;    //will always be at least 1 byte long i.e. for numbers between 0-255

            do
            {
                value >>= 7;    //get next 7 bits

                if (value > 0)
                {
                    length++;   ///need another byte
                }
            }
            while (value > 0);

            return length;
        }

        public static uint ReadVariableLengthValue(BinaryReader reader)
        {
            //Debug.WriteLine("========= READ VLV =========");

            if (reader == null)
                throw new ArgumentNullException("reader");

            bool more = true;
            int value = 0;
            int shift = 0;

            List<Byte> encodedBytes = new List<byte>();
            List<Byte> decodedBytes = new List<byte>();

            //this loop simply reads the bytes in - it does not decode them
            while (more)
            {
                byte lower7bits = reader.ReadByte();
                encodedBytes.Add(lower7bits);
                more = (lower7bits & 128) != 0;
                decodedBytes.Add(lower7bits);
            }

            if (BitConverter.IsLittleEndian)
                encodedBytes.Reverse();

            if (BitConverter.IsLittleEndian)
                decodedBytes.Reverse();

            //Debug.WriteLine("Big Endian Bits");
            //ShowBits2(encodedBytes.ToArray());

            foreach (var b in decodedBytes)
            {
                value |= ((b & 0x7f) << shift); //get lower 7 bits of byte and store them in the next 7 bits of our value. we have to shift left to set the more significant bytes
                shift += 7;
            }

            //Byte[] originalBytes = BitConverter.GetBytes(value);
            //Debug.WriteLine("Little Endian Bits");
            //ShowBits2(originalBytes);
            //Debug.WriteLine(String.Format("VLV: {0}", value));

            return (uint)value;
        }

        public static void WriteVariableLengthValue(BinaryWriter writer, uint value)
        {
            //Debug.WriteLine("========= WRITE VLV =========");

            if (writer == null)
                throw new ArgumentNullException("writer");

            //Debug.WriteLine(String.Format("VLV: {0}", value));

            List<Byte> encodedBytes = new List<byte>();

            //List<Byte> decodedBytes = new List<byte>(BitConverter.GetBytes(value)); // always 4
            //Debug.WriteLine("Decoded Bits");
            //ShowBitsBrokenInToBytes(decodedBytes.ToArray(), true);

            uint result = value;

            encodedBytes.Add((byte)(result & 0x7F));

            //the problem is that the bytes are in little endian and so we end up shifting to the left
            //this algorthm assume that we have the number in big endian
            while (result >= 128)
            {
                result = result >> 7;
                encodedBytes.Add((byte)((result & 0x7F) | 0x80));
            }

            if (BitConverter.IsLittleEndian)
                encodedBytes.Reverse();

            //Debug.WriteLine("Encoded Bits");
            //ShowBitsBrokenInToBytes(encodedBytes.ToArray(), true);
            //Debug.Assert(encodedBytes.Count == GetLengthOfVariableLengthValue(value));

            writer.Write(encodedBytes.ToArray());
        }
    }
}