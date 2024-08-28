using LibellusLibrary.JSON;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types.Frame
{
	internal class PmdTarget_Se : P3TargetType
	{
		[JsonPropertyOrder(-92)]
		public short SoundIndex { get; set; } // limited 0-10000 in editor
		
		[JsonPropertyOrder(-91)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public SEModeEnum SoundMode { get; set; }

		[JsonPropertyOrder(-90)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		internal enum SEModeEnum : byte
		{
			PLAY_CH0 = 0,
			STOP_CH0 = 1,
			PLAY_CH1 = 2,
			STOP_CH1 = 3
		}

		protected override void ReadData(BinaryReader reader)
		{
			SoundIndex = reader.ReadInt16();
			SoundMode = (SEModeEnum)reader.ReadByte();
			Data = reader.ReadBytes(37);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write(SoundIndex);
			writer.Write((byte)SoundMode);
			writer.Write(Data);
		}
	}
}
