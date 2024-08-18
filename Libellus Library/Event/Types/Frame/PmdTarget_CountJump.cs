using LibellusLibrary.JSON;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types.Frame
{
	internal class PmdTarget_CountJump : PmdTargetType
	{
		[JsonPropertyOrder(-92)]
		public sbyte CounterNumber { get; set; } // Limited 0-8 in editor

		[JsonPropertyOrder(-91)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		[JsonPropertyOrder(-90)]
		public byte CompareValueOne { get; set; } // limited 0-250 in editor

		[JsonPropertyOrder(-89)]
		public byte CompareValueTwo { get; set; } // limited 0-250 in editor

		[JsonPropertyOrder(-88)]
		public byte CompareValueThree { get; set; } // limited 0-250 in editor

		[JsonPropertyOrder(-87)]
		public byte Field1B { get; set; }

		[JsonPropertyOrder(-86)]
		public short JumpOne { get; set; } // If (CounterNumber == CompareValueOne) jump here?; limited to positive values only (0x0-0x7FFF)

		[JsonPropertyOrder(-85)]
		public short JumpTwo { get; set; } // If (CounterNumber == CompareValueTwo) jump here?; limited to positive values only (0x0-0x7FFF)

		[JsonPropertyOrder(-84)]
		public short JumpThree { get; set; } // If (CounterNumber == CompareValueThree) jump here?; limited to positive values only (0x0-0x7FFF)

		[JsonPropertyOrder(-83)]
		public short JumpFour { get; set; } // Jump here if all counter comparisions fail?; limited to positive values only (0x0-0x7FFF)

		[JsonPropertyOrder(-82)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data2 { get; set; } = Array.Empty<byte>();

		protected override void ReadData(BinaryReader reader)
		{
			CounterNumber = reader.ReadSByte();
			Data = reader.ReadBytes(3);
			CompareValueOne = reader.ReadByte();
			CompareValueTwo = reader.ReadByte();
			CompareValueThree = reader.ReadByte();
			Field1B = reader.ReadByte();
			JumpOne = reader.ReadInt16();
			JumpTwo = reader.ReadInt16();
			JumpThree = reader.ReadInt16();
			JumpFour = reader.ReadInt16();
			Data2 = reader.ReadBytes(24);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write(CounterNumber);
			writer.Write(Data);
			writer.Write(CompareValueOne);
			writer.Write(CompareValueTwo);
			writer.Write(CompareValueThree);
			writer.Write(Field1B);
			writer.Write(JumpOne);
			writer.Write(JumpTwo);
			writer.Write(JumpThree);
			writer.Write(JumpFour);
			writer.Write(Data2);
		}
	}
}
