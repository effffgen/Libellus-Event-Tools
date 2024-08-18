using LibellusLibrary.JSON;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types.Frame
{
	internal class PmdTarget_Keyfree : PmdTargetType
	{
		[JsonPropertyOrder(-92)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public KeyfreeModeEnum Mode { get; set; }

		[JsonPropertyOrder(-91)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		[JsonPropertyOrder(-90)]
		public short StartPosition { get; set; } // Limited 0-127 in editor

		[JsonPropertyOrder(-89)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data2 { get; set; } = Array.Empty<byte>();

		internal enum KeyfreeModeEnum : byte
		{
			NO_KEYFREE = 0,
			KEYFREE = 1
		}

		protected override void ReadData(BinaryReader reader)
		{
			Mode = (KeyfreeModeEnum)reader.ReadByte();
			Data = reader.ReadBytes(3);
			StartPosition = reader.ReadInt16();
			Data2 = reader.ReadBytes(34);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write((byte)Mode);
			writer.Write(Data);
			writer.Write(StartPosition);
			writer.Write(Data2);
		}
	}
}
