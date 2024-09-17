using LibellusLibrary.JSON;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types.Frame
{
	// Cond == Condition
	internal class P3Target_CondOn : P3TargetType
	{
		[JsonPropertyOrder(-92)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public CondTypeEnum CondMode { get; set; }

		[JsonPropertyOrder(-91)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		internal enum CondTypeEnum : byte
		{
			KAZE = 0, // Sick? likely 風邪 which roughly means "flu"
			KAZEGIMI = 1,
			HIROU = 2, // Tired? likely 疲労 which roughly means "fatigue"
			TUUJOU = 3, // Good? likely 通常 which roughly means "normal"
			KOUTYOU = 4,
			ZEKKOUTYOU = 5 // Likely 絶好調 which roughly means "in perfect form"
		}

		protected override void ReadData(BinaryReader reader)
		{
			CondMode = (CondTypeEnum)reader.ReadByte();
			Data = reader.ReadBytes(39);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write((byte)CondMode);
			writer.Write(Data);
		}
	}
}
