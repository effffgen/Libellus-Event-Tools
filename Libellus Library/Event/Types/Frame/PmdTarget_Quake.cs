using LibellusLibrary.JSON;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types.Frame
{
    internal class PmdTarget_Quake : PmdTargetType
    {
        [JsonPropertyOrder(-92)]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public short Range { get; set; }

        [JsonPropertyOrder(-91)]
        [JsonConverter(typeof(ByteArrayToHexArray))]
        public byte[] Data { get; set; }

        protected override void ReadData(BinaryReader reader)
        {
            Range = reader.ReadInt16();
            Data = reader.ReadBytes(38);
        }

        protected override void WriteData(BinaryWriter writer)
        {
            writer?.Write((short)Range);
            writer?.Write(Data);
        }
    }
}
