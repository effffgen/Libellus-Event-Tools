using LibellusLibrary.JSON;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types.Frame
{
    internal class PmdTarget_Message : PmdTargetType
    {
        [JsonPropertyOrder(-100)]
        [JsonConverter(typeof(ByteArrayToHexArray))]
        public byte[] Data { get; set; }

        // [JsonPropertyOrder(-99)]
        // public PmdFlags Flags { get; set; }

        [JsonPropertyOrder(-99)]
        public uint MessageIndex { get; set; }

        [JsonPropertyOrder(-98)]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MessageModeEnum MessageMode { get; set; }

        [JsonPropertyOrder(-97)]
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
            //Flags = new PmdFlags();
            //Flags.ReadData(reader);
            MessageIndex = reader.ReadUInt32();
            MessageMode = (MessageModeEnum)reader.ReadUInt32();
            Data2 = reader.ReadBytes(32);
        }

        protected override void WriteData(BinaryWriter writer)
        {
            writer?.Write(Data);
            // Flags.WriteData(writer);
            writer?.Write(MessageIndex);
            writer?.Write((uint)MessageMode);
            writer?.Write(Data2);
        }

    }
}
