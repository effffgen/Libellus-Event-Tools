using LibellusLibrary.JSON;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types.Frame
{
	internal class P4Target_FldNoise : P3TargetType
	{
		[JsonPropertyOrder(-92)]
		public byte NoiseEnabled { get; set; }

		[JsonPropertyOrder(-91)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		protected override void ReadData(BinaryReader reader)
		{
			NoiseEnabled = reader.ReadByte();
			Data = reader.ReadBytes(39);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write(NoiseEnabled);
			writer.Write(Data);
		}
	}
}
