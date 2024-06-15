using LibellusLibrary.JSON;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types.Frame
{
	internal class PmdTarget_Cutin : PmdTargetType
	{
		[JsonPropertyOrder(-92)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public CutinModeEnum CutinMode { get; set; }
		
		[JsonPropertyOrder(-91)]
		public byte Field01 { get; set; } // Character/unit id? Group (as in which KOMA entry is it listed under in editor) id?
		
		[JsonPropertyOrder(-90)]
		public ushort Field02 { get; set; }
		
		[JsonPropertyOrder(-89)]
		public short CHARA { get; set; }
		
		[JsonPropertyOrder(-88)]
		public short FACE { get; set; }
		
		[JsonPropertyOrder(-87)]
		public ushort FUKU { get; set; }
		
		[JsonPropertyOrder(-86)]
		public short SYURUI { get; set; }
		
		[JsonPropertyOrder(-85)]
		public short X { get; set; }
		
		[JsonPropertyOrder(-84)]
		public short Y { get; set; }
		
		[JsonPropertyOrder(-83)]
		public CutinTypeEnum TYPE { get; set; }
		
		[JsonPropertyOrder(-82)]
		public byte Field0E { get; set; } // unused?
		
		[JsonPropertyOrder(-81)]
		public byte REFNO { get; set; } // always shown as x in editor, always editable however; limited to 0-9

		[JsonPropertyOrder(-80)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		internal enum CutinModeEnum : byte
		{
			LOAD = 0,
			START = 1,
			RELEASE = 2,
		}
		
		internal enum CutinTypeEnum : byte
		{
			DIRECT = 0,
			REF = 1,
		}

		protected override void ReadData(BinaryReader reader)
		{
			CutinMode = (CutinModeEnum)reader.ReadByte();
			Field01 = reader.ReadByte();
			Field02 = reader.ReadUInt16();
			CHARA = reader.ReadInt16();
			FACE = reader.ReadInt16();
			FUKU = reader.ReadUInt16();
			SYURUI = reader.ReadInt16();
			X = reader.ReadInt16();
			Y = reader.ReadInt16();
			TYPE = (CutinTypeEnum)reader.ReadByte();
			Field0E = reader.ReadByte();
			REFNO = reader.ReadByte();
			Data = reader.ReadBytes(21);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer?.Write((byte)CutinMode);
			writer?.Write(Field01);
			writer?.Write(Field02);
			writer?.Write(CHARA);
			writer?.Write(FACE);
			writer?.Write(FUKU);
			writer?.Write(SYURUI);
			writer?.Write(X);
			writer?.Write(Y);
			writer?.Write((byte)TYPE);
			writer?.Write(Field0E);
			writer?.Write(REFNO);
			writer?.Write(Data);
		}
	}
}
