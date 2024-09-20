using LibellusLibrary.JSON;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types.Frame
{
	internal class P3Target_SpuSe : P3TargetType
	{
		[JsonPropertyOrder(-92)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public SpuEnum SpuType { get; set; }

		[JsonPropertyOrder(-91)]
		public sbyte ChannelNumber { get; set; }

		[JsonPropertyOrder(-90)]
		public sbyte SetNumber { get; set; }

		[JsonPropertyOrder(-89)]
		public sbyte SequenceNumber { get; set; }

		[JsonPropertyOrder(-88)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		internal enum SpuEnum : byte
		{
			BSE = 0,
			COMSE = 1
		}

		protected override void ReadData(BinaryReader reader)
		{
			SpuType = (SpuEnum)reader.ReadByte();
			ChannelNumber = reader.ReadSByte();
			SetNumber = reader.ReadSByte();
			SequenceNumber = reader.ReadSByte();
			Data = reader.ReadBytes(36);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write((byte)SpuType);
			writer.Write(ChannelNumber);
			writer.Write(SetNumber);
			writer.Write(SequenceNumber);
			writer.Write(Data);
		}
	}
}
