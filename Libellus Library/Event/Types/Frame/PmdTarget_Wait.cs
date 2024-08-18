using LibellusLibrary.JSON;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types.Frame
{
	internal class PmdTarget_Wait : PmdTargetType
	{
		[JsonPropertyOrder(-92)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public WaitModeEnum WaitMode { get; set; }

		[JsonPropertyOrder(-91)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		internal enum WaitModeEnum : byte
		{
			MESSAGE_WAIT = 0, // Pauses event playback until msg window is closed (used for MESSAGE calls set to NO STOP)
			FADESYNC_WAIT = 1
		}

		protected override void ReadData(BinaryReader reader)
		{
			WaitMode = (WaitModeEnum)reader.ReadByte();
			Data = reader.ReadBytes(39);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write((byte)WaitMode);
			writer.Write(Data);
		}
	}
}
