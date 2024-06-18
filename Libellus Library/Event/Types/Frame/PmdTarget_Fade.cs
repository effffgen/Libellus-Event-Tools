using LibellusLibrary.JSON;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types.Frame
{
	internal class PmdTarget_Fade : PmdTargetType
	{
		[JsonPropertyOrder(-92)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public FadeModeEnum FadeMode { get; set; }

		[JsonPropertyOrder(-91)]
		public short FadeNumber { get; set; }

		[JsonPropertyOrder(-90)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		internal enum FadeModeEnum : ushort
		{
			FADE_IN = 0,
			FADE_OUT = 1,
		}

		protected override void ReadData(BinaryReader reader)
		{
			FadeMode = (FadeModeEnum)reader.ReadUInt16();
			FadeNumber = reader.ReadInt16();
			Data = reader.ReadBytes(36);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer?.Write((ushort)FadeMode);
			writer?.Write((short)FadeNumber);
			writer?.Write(Data);
		}
	}
}
