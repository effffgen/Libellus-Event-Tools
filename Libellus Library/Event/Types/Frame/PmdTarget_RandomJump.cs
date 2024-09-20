using LibellusLibrary.JSON;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types.Frame
{
	internal class P3Target_RandomJump : P3TargetType
	{

		[JsonPropertyOrder(-92)]
		public sbyte ChanceOne { get; set; } // Percentage chance of jumping to JumpFrameOne; KAKURITU0 (likely 確率 roughly meaning probability)+limited 0-100 in editor

        [JsonPropertyOrder(-89)]
		public sbyte ChanceTwo { get; set; } // KAKURITU1+limited 0-100 in editor

		[JsonPropertyOrder(-88)]
		public sbyte ChanceThree { get; set; } // KAKURITU2+limited 0-100 in editor

		[JsonPropertyOrder(-87)]
		public sbyte ChanceFour { get; set; } // KAKURITU3+limited 0-100 in editor

		[JsonPropertyOrder(-91)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		[JsonPropertyOrder(-86)]
		public short JumpFrameOne { get; set; } // limited 0-30000 in editor

		[JsonPropertyOrder(-85)]
		public short JumpFrameTwo { get; set; } // limited 0-30000 in editor

		[JsonPropertyOrder(-84)]
		public short JumpFrameThree { get; set; } // limited 0-30000 in editor

		[JsonPropertyOrder(-83)]
		public short JumpFrameFour { get; set; } // limited 0-30000 in editor

		[JsonPropertyOrder(-82)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data2 { get; set; } = Array.Empty<byte>();

		protected override void ReadData(BinaryReader reader)
		{
			ChanceOne = reader.ReadSByte();
			ChanceTwo = reader.ReadSByte();
			ChanceThree = reader.ReadSByte();
			ChanceFour = reader.ReadSByte();
			Data = reader.ReadBytes(4);
			JumpFrameOne = reader.ReadInt16();
			JumpFrameTwo = reader.ReadInt16();
			JumpFrameThree = reader.ReadInt16();
			JumpFrameFour = reader.ReadInt16();
			Data2 = reader.ReadBytes(24);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write(ChanceOne);
			writer.Write(ChanceTwo);
			writer.Write(ChanceThree);
			writer.Write(ChanceFour);
			writer.Write(Data);
			writer.Write(JumpFrameOne);
			writer.Write(JumpFrameTwo);
			writer.Write(JumpFrameThree);
			writer.Write(JumpFrameFour);
			writer.Write(Data2);
		}
	}
}
