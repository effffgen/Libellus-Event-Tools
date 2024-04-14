using LibellusLibrary.JSON;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types.Frame
{
    internal class PmdTarget_Fade : PmdTargetType
    {
        [JsonPropertyOrder(-92)]
        public ushort FadeType { get; set; }
		
		[JsonPropertyOrder(-91)]
        public short FadeLength { get; set; }

        [JsonPropertyOrder(-90)]
        [JsonConverter(typeof(ByteArrayToHexArray))]
        public byte[] Data { get; set; }

        protected override void ReadData(BinaryReader reader)
        {
            FadeType = reader.ReadUInt16();
            FadeLength = reader.ReadInt16();
            Data = reader.ReadBytes(36);
        }

        protected override void WriteData(BinaryWriter writer)
        {
            writer?.Write((ushort)FadeType);
            writer?.Write((short)FadeLength);
            writer?.Write(Data);
        }
    }
}
