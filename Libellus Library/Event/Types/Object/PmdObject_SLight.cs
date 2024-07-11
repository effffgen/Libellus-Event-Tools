using LibellusLibrary.JSON;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types.Object
{
	internal class PmdObject_SLight : PmdObjectType
	{
		[JsonPropertyOrder(-96)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public SLightTypeEnum SLightType { get; set; }
		[JsonPropertyOrder(-95)]
		public byte Field07 { get; set; } // Unknown; possibly unused
		[JsonPropertyOrder(-94)]
		public ushort ResourceIndex { get; set; } // Listed under SLightType as "resId 0x*" in editor
		[JsonPropertyOrder(-93)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();
		[JsonPropertyOrder(-92)]
		public uint Field10 { get; set; }

		internal enum SLightTypeEnum : sbyte
		{
			DISABLE = 0,
			ALL = 1,
			UNIT_ALL = 2,
			resId = 3
		}
		protected override void ReadData(BinaryReader reader)
		{
			SLightType = (SLightTypeEnum)reader.ReadSByte();
			Field07 = reader.ReadByte();
			ResourceIndex = reader.ReadUInt16();
			Data = reader.ReadBytes(6);
			Field10 = reader.ReadUInt32();
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write((sbyte)SLightType);
			writer.Write(Field07);
			writer.Write(ResourceIndex);
			writer.Write(Data);
			writer.Write(Field10);
		}
	}
}
