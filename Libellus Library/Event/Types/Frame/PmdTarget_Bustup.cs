using LibellusLibrary.JSON;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types.Frame
{
	internal class P3Target_Bustup : P3TargetType, ITargetVarying
	{
		[JsonPropertyOrder(-92)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public BupControlEnum BustupControlMode { get; set; } // Read as byte in editor but stored in file as uint

		internal enum BupControlEnum : uint
		{
			LOAD = 0,
			FADEOUT = 1, // Stores no actual data?
			KUCHI = 2,
			MEPACHI = 3 // Likely 目パチ roughly meaning "blinking eyes"
		}

		public PmdTargetType GetVariant(BinaryReader reader)
		{
			reader.BaseStream.Position += 18;
			return GetBustup((BupControlEnum)reader.ReadUInt32());
		}

		public PmdTargetType GetVariant()
		{
			return GetBustup(BustupControlMode);
		}

		public static PmdTargetType GetBustup(BupControlEnum mode) => mode switch
		{
			BupControlEnum.LOAD => new LoadBustup(),
			BupControlEnum.KUCHI or BupControlEnum.MEPACHI => new UnknownToggleBustup(),
			_ => new UnknownBustup()
		};
	}

	internal class UnknownBustup : P3Target_Bustup
	{
		[JsonPropertyOrder(-91)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		protected override void ReadData(BinaryReader reader)
		{
			BustupControlMode = (BupControlEnum)reader.ReadUInt32();
			Data = reader.ReadBytes(36);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write((uint)BustupControlMode);
			writer.Write(Data);
		}
	}

	internal class LoadBustup : P3Target_Bustup
	{
		[JsonPropertyOrder(-91)]
		public short X { get; set; } // Limited -1024-1024 in editor

		[JsonPropertyOrder(-90)]
		public short Y { get; set; } // Limited -1024-1024 in editor

		[JsonPropertyOrder(-89)]
		public short BustupIndex { get; set; } // limited 0-100 in editor

		[JsonPropertyOrder(-88)]
		public short FUKU { get; set; } // limited 0x0-0xF in editor

		[JsonPropertyOrder(-87)]
		public short HYOUJOU { get; set; } // limited 0-100 in editor

		[JsonPropertyOrder(-86)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		protected override void ReadData(BinaryReader reader)
		{
			BustupControlMode = (BupControlEnum)reader.ReadUInt32();
			X = reader.ReadInt16();
			Y = reader.ReadInt16();
			BustupIndex = reader.ReadInt16();
			FUKU = reader.ReadInt16();
			HYOUJOU = reader.ReadInt16();
			Data = reader.ReadBytes(26);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write((uint)BustupControlMode);
			writer.Write(X);
			writer.Write(Y);
			writer.Write(BustupIndex);
			writer.Write(FUKU);
			writer.Write(HYOUJOU);
			writer.Write(Data);
		}
	}

	internal class UnknownToggleBustup : P3Target_Bustup
	{
		[JsonPropertyOrder(-91)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public OffOnEnum ToggleState { get; set; }

		[JsonPropertyOrder(-90)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		internal enum OffOnEnum : byte
		{
			OFF = 0,
			ON = 1
		}

		protected override void ReadData(BinaryReader reader)
		{
			BustupControlMode = (BupControlEnum)reader.ReadUInt32();
			ToggleState = (OffOnEnum)reader.ReadByte();
			Data = reader.ReadBytes(35);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write((uint)BustupControlMode);
			writer.Write((byte)ToggleState);
			writer.Write(Data);
		}
	}
}
