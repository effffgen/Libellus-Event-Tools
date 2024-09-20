using LibellusLibrary.JSON;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types.Frame
{
	internal class P4Target_CameraState : P3TargetType
	{
		[JsonPropertyOrder(-92)]
		public float CameraNear { get; set; } // Assumed, not entirely certain
		
		[JsonPropertyOrder(-91)]
		public float CameraFar { get; set; } // Assumed, not entirely certain

		[JsonPropertyOrder(-90)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		protected override void ReadData(BinaryReader reader)
		{
			CameraNear = reader.ReadSingle();
			CameraFar = reader.ReadSingle();
			Data = reader.ReadBytes(32);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write(CameraNear);
			writer.Write(CameraFar);
			writer.Write(Data);
		}
	}
}
