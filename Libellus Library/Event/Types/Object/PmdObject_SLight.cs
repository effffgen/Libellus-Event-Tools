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
		public byte ResourceIndex { get; set; } // Listed under SLightType as "resId 0x*" in editor
		// resId values seem to be formatted as ID/Index byte, followed by a byte denoting a "type"
		// so for example, Index 142 of type PackModel would be 0x8E0F, and Index 8 of type FBN would be 0x080C (in editor these would be 0xf8e and 0xc08)
		// TODO: Is this actually how it works for certain?
		[JsonPropertyOrder(-93)]
		public byte ResourceType { get; set; }
		[JsonPropertyOrder(-92)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();
		[JsonPropertyOrder(-91)]
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
			ResourceIndex = reader.ReadByte();
			ResourceType = reader.ReadByte();
			Data = reader.ReadBytes(6);
			Field10 = reader.ReadUInt32();
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write((sbyte)SLightType);
			writer.Write(Field07);
			writer.Write(ResourceIndex);
			writer.Write(ResourceType);
			writer.Write(Data);
			writer.Write(Field10);
		}
	}
}
