using LibellusLibrary.JSON;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types.Frame
{
    internal class PmdTarget_Message : PmdTargetType
    {
        [JsonPropertyOrder(-100)]
        public byte MessageIndex { get; set; }
        
        [JsonPropertyOrder(-99)]
        public byte SetLocalFlag { get; set; }

        [JsonPropertyOrder(-98)]
        public ushort Field16 { get; set; }

        [JsonPropertyOrder(-97)]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MessageModeEnum MessageMode { get; set; }

        [JsonPropertyOrder(-96)]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public AccessModeEnum MessageAccessMode { get; set; }

        [JsonPropertyOrder(-95)]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DisplayModeEnum MessageDisplayMode { get; set; }

        [JsonPropertyOrder(-94)]
        [JsonConverter(typeof(ByteArrayToHexArray))]
        public byte[] Data2 { get; set; }

        internal enum MessageModeEnum : byte
        {
            STOP = 0,
            NO_STOP = 1,
        }

        internal enum AccessModeEnum : byte
        {
            DIRECT = 0,
            REF0 = 1,
            REF1 = 2,
            REF2 = 3,
            REF3 = 4,
            REF4 = 5,
            JOUTYUU = 6,
        }

        internal enum DisplayModeEnum : byte
        {
            NORMAL = 0,
            TUTORIAL = 1,
        }

        protected override void ReadData(BinaryReader reader)
        {
            MessageIndex = reader.ReadByte();
            SetLocalFlag = reader.ReadByte();
            Field16 = reader.ReadUInt16();
            MessageMode = (MessageModeEnum)reader.ReadByte();
            MessageAccessMode = (AccessModeEnum)reader.ReadByte();
            MessageDisplayMode = (DisplayModeEnum)reader.ReadByte();
            Data2 = reader.ReadBytes(33);
        }

        protected override void WriteData(BinaryWriter writer)
        {
            writer?.Write((byte)MessageIndex);
            writer?.Write((byte)SetLocalFlag);
            writer?.Write((ushort)Field16);
            writer?.Write((byte)MessageMode);
            writer?.Write((byte)MessageAccessMode);
            writer?.Write((byte)MessageDisplayMode);
            writer?.Write(Data2);
        }

    }
}
