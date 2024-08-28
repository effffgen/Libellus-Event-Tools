using LibellusLibrary.JSON;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types.Frame
{
	internal class PmdTarget_Movie : P3TargetType
	{
		[JsonPropertyOrder(-92)]
		public short MovieIndex { get; set; } // limited 0-180 in editor

		[JsonPropertyOrder(-91)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		protected override void ReadData(BinaryReader reader)
		{
			MovieIndex = reader.ReadInt16();
			Data = reader.ReadBytes(38);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write(MovieIndex);
			writer.Write(Data);
		}
	}
}
