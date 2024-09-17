using LibellusLibrary.JSON;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types.Frame
{
	// Cond == Condition
	internal class P3Target_CondJump : P3TargetType
	{
		[JsonPropertyOrder(-92)]
		public short KazeFrame { get; set; } // Goto frame if condition == 0 == sick?

		[JsonPropertyOrder(-91)]
		public short KazegimiFrame { get; set; } // Goto frame if condition == 1

		[JsonPropertyOrder(-90)]
		public short HirouFrame { get; set; } // Goto frame if condition == 2 == tired?

		[JsonPropertyOrder(-89)]
		public short TuujouFrame { get; set; } // Goto frame if condition == 3 == good?

		[JsonPropertyOrder(-88)]
		public short KoutyouFrame { get; set; } // Goto frame if condition == 4

		[JsonPropertyOrder(-87)]
		public short ZekkoutyouFrame { get; set; } // Goto frame if condition == 5

		[JsonPropertyOrder(-86)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		protected override void ReadData(BinaryReader reader)
		{
			KazeFrame = reader.ReadInt16();
			KazegimiFrame = reader.ReadInt16();
			HirouFrame = reader.ReadInt16();
			TuujouFrame = reader.ReadInt16();
			KoutyouFrame = reader.ReadInt16();
			ZekkoutyouFrame = reader.ReadInt16();
			Data = reader.ReadBytes(28);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write(KazeFrame);
			writer.Write(KazegimiFrame);
			writer.Write(HirouFrame);
			writer.Write(TuujouFrame);
			writer.Write(KoutyouFrame);
			writer.Write(ZekkoutyouFrame);
			writer.Write(Data);
		}
	}
}
