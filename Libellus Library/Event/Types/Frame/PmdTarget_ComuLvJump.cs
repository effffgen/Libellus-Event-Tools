using LibellusLibrary.JSON;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types.Frame
{
	internal class P3Target_ComuLvJump : P3TargetType
	{
		[JsonPropertyOrder(-92)]
		public short Jump1 { get; set; } // listed with (comuLv x 5Per) in editor; jump to this frame with a chance of SL rank * 5%?
		
		[JsonPropertyOrder(-91)]
		public short Jump2 { get; set; }

		[JsonPropertyOrder(-90)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		protected override void ReadData(BinaryReader reader)
		{
			Jump1 = reader.ReadInt16();
			Jump2 = reader.ReadInt16();
			Data = reader.ReadBytes(36);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write(Jump1);
			writer.Write(Jump2);
			writer.Write(Data);
		}
	}
}
