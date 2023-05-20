using LibellusLibrary.JSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LibellusLibrary.Event.Types.Frame
{
    internal class PmdTarget_Message : PmdTargetType
    {
        [JsonPropertyOrder(-100)]
        [JsonConverter(typeof(ByteArrayToHexArray))]
        public byte[] Data { get; set; }

        [JsonPropertyOrder(-99)]
        public PmdMessageFlags Flags { get; set; }

        [JsonPropertyOrder(-98)]
        public uint MessageIndex { get; set; }

        [JsonPropertyOrder(-97)]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MessageModeEnum MessageMode { get; set; }

        [JsonPropertyOrder(-96)]
        [JsonConverter(typeof(ByteArrayToHexArray))]
        public byte[] Data2 { get; set; }

        internal enum MessageModeEnum : uint
        {
            STOP = 0,
            NO_STOP = 1,
        }

        protected override void ReadData(BinaryReader reader)
        {
            Data = reader.ReadBytes(4);
            Flags = new PmdMessageFlags();
            Flags.ReadData(reader);
            MessageIndex = reader.ReadUInt32();
            MessageMode = (MessageModeEnum)reader.ReadUInt32();
            Data2 = reader.ReadBytes(32);
        }

        protected override void WriteData(BinaryWriter writer)
        {
            writer?.Write(Data);
            Flags.WriteData(writer);
            writer?.Write(MessageIndex);
            writer?.Write((uint)MessageMode);
            writer?.Write(Data2);
        }

    }

    internal class PmdMessageFlags
    {
        [JsonPropertyOrder(-100)]
        public ushort FlagNumber { get; set; }

        [JsonPropertyOrder(-99)]
        public ushort CmpValue { get; set; }

        [JsonPropertyOrder(-98)]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UnitFlagType FlagType { get; set; }

        [JsonPropertyOrder(-97)]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UnitGFlagType GFlagType { get; set; }

        internal enum UnitFlagType : short
        {
            DISABLE = 0,
            LOCAL = 1,
            GLOBAL = 2,
        }

        internal enum UnitGFlagType : short
        {
            EVT = 0,
            COMMU = 1,
            SYS = 2,
        }

        internal void ReadData(BinaryReader reader)
        {
            FlagNumber = reader.ReadUInt16();
            CmpValue = reader.ReadUInt16();
            FlagType = (UnitFlagType)reader.ReadUInt16();
            GFlagType = (UnitGFlagType)reader.ReadUInt16();
        }

        internal void WriteData(BinaryWriter writer)
        {
            writer?.Write((ushort)FlagNumber);
            writer?.Write((ushort)CmpValue);
            writer?.Write((ushort)FlagType);
            writer?.Write((ushort)GFlagType);
        }

    }
}
