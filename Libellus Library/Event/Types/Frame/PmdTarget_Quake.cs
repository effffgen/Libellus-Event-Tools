using LibellusLibrary.JSON;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types.Frame
{
	internal class SMT3Target_Quake : SMT3TargetType
	{
		[JsonPropertyOrder(-96)]
		public short Range { get; set; }

		[JsonPropertyOrder(-95)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		protected override void ReadData(BinaryReader reader)
		{
			Range = reader.ReadInt16();
			Data = reader.ReadBytes(6);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write(Range);
			writer.Write(Data);
		}
	}

	internal class DDSTarget_Quake : DDSTargetType
	{
		[JsonPropertyOrder(-95)]
		public short Range { get; set; }

		[JsonPropertyOrder(-94)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		protected override void ReadData(BinaryReader reader)
		{
			Range = reader.ReadInt16();
			Data = reader.ReadBytes(0x1E);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write(Range);
			writer.Write(Data);
		}
	}

	internal class P3Target_Quake : P3TargetType
	{
		[JsonPropertyOrder(-92)]
		public short Range { get; set; }

		[JsonPropertyOrder(-91)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		protected override void ReadData(BinaryReader reader)
		{
			Range = reader.ReadInt16();
			Data = reader.ReadBytes(38);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write(Range);
			writer.Write(Data);
		}
	}
}
