using LibellusLibrary.JSON;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types.Frame
{
	internal class DDSTarget_Rain : DDSTargetType
	{
		[JsonPropertyOrder(-95)]
		public ushort Field0C { get; set; }

		[JsonPropertyOrder(-94)]
		public ushort RainDataIndex { get; set; } // Assumed

		[JsonPropertyOrder(-93)]
		public ushort Field10 { get; set; }

		[JsonPropertyOrder(-92)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		protected override void ReadData(BinaryReader reader)
		{
			Field0C = reader.ReadUInt16();
			RainDataIndex = reader.ReadUInt16();
			Field10 = reader.ReadUInt16();
			Data = reader.ReadBytes(0x1A);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write(Field0C);
			writer.Write(RainDataIndex);
			writer.Write(Field10);
			writer.Write(Data);
		}
	}
}
