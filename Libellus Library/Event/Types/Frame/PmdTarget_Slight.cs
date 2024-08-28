using LibellusLibrary.JSON;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types.Frame
{
	internal class P3Target_Slight : P3TargetType
	{
		[JsonPropertyOrder(-92)]
		public float Field14 { get; set; } // Possibly a seperate length value?
		[JsonPropertyOrder(-92)]
		public uint Field18 { get; set; }
		[JsonPropertyOrder(-92)]
		public ushort EditorGroup { get; set; } // Uncertain but most likely what this does
		[JsonPropertyOrder(-92)]
		public ushort SLightDataIndex { get; set; }

		[JsonPropertyOrder(-91)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		protected override void ReadData(BinaryReader reader)
		{
			Field14 = reader.ReadSingle();
			Field18 = reader.ReadUInt32();
			EditorGroup = reader.ReadUInt16();
			SLightDataIndex = reader.ReadUInt16();
			Data = reader.ReadBytes(28);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write(Field14);
			writer.Write(Field18);
			writer.Write(EditorGroup);
			writer.Write(SLightDataIndex);
			writer.Write(Data);
		}
	}

	internal class DDSTarget_Slight : DDSTargetType
	{
		[JsonPropertyOrder(-95)]
		public float Field14 { get; set; }
		[JsonPropertyOrder(-94)]
		public float Field18 { get; set; } // Possibly a float in P3Target_Slight as well?
		[JsonPropertyOrder(-93)]
		public ushort EditorGroup { get; set; }
		[JsonPropertyOrder(-92)]
		public ushort SLightDataIndex { get; set; }

		[JsonPropertyOrder(-91)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		protected override void ReadData(BinaryReader reader)
		{
			Field14 = reader.ReadSingle();
			Field18 = reader.ReadSingle();
			EditorGroup = reader.ReadUInt16();
			SLightDataIndex = reader.ReadUInt16();
			Data = reader.ReadBytes(0x14);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write(Field14);
			writer.Write(Field18);
			writer.Write(EditorGroup);
			writer.Write(SLightDataIndex);
			writer.Write(Data);
		}
	}
}
