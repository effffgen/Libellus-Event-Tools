using LibellusLibrary.JSON;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types.Object
{
	internal class PmdObject_PackModel : PmdObjectType
	{
		[JsonPropertyOrder(-96)]
		public sbyte NameIndex { get; set; } // IDX in editor
		[JsonPropertyOrder(-95)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();
		[JsonPropertyOrder(-94)]
		public uint Field10 { get; set; }

		protected override void ReadData(BinaryReader reader)
		{
			NameIndex = reader.ReadSByte();
			Data = reader.ReadBytes(9);
			Field10 = reader.ReadUInt32();
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write(NameIndex);
			writer.Write(Data);
			writer.Write(Field10);
		}
	}
}
