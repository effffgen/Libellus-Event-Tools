using LibellusLibrary.JSON;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types.Frame
{
	internal class P3Target_CustomEvent : P3TargetType, ITargetVarying
	{
		[JsonPropertyOrder(-92)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public EventModeEnum EventMode { get; set; }

		// PERU == Persona?
		internal enum EventModeEnum : uint
		{
			HIRU_SASO = 0, // Likely means roughly "Invited at noon"
			BASHO2SAS = 1,
			PARA_UPDW = 2, // Change player stats
			HOLYHOGO = 3,
			HANDENWA = 4,
			UWAKI_HAK = 5,
			MORAU_PRE = 6, // Likely 貰う PRE which roughly means "Receive Present"?
			AGERU_PRE = 7, // Likely 上げる PRE which roughly means "Give Present"?
			HIDUKEMES = 8,
			CARDEFF = 9,
			GAMEOVER = 10,
			HAVEPERU = 11,
			LVUPMES = 12,
			PERUKOE = 13,
		}

		public PmdTargetType GetVariant(BinaryReader reader)
		{
			reader.BaseStream.Position += 18;
			return GetCustomEvent((EventModeEnum)reader.ReadUInt32());
		}

		public PmdTargetType GetVariant()
		{
			return GetCustomEvent(EventMode);
		}

		public static PmdTargetType GetCustomEvent(EventModeEnum mode) => mode switch
		{
			EventModeEnum.HIRU_SASO or EventModeEnum.HOLYHOGO or EventModeEnum.HANDENWA or EventModeEnum.UWAKI_HAK => new HiruSaso(),
			EventModeEnum.PARA_UPDW => new StatChange(),
			EventModeEnum.MORAU_PRE => new MorauPre(),
			EventModeEnum.AGERU_PRE => new AgeruPre(),
			EventModeEnum.CARDEFF => new CardEff(),
			_ => new UnknownEvent()
		};
	}

	internal class UnknownEvent : P3Target_CustomEvent
	{
		[JsonPropertyOrder(-91)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		protected override void ReadData(BinaryReader reader)
		{
			EventMode = (EventModeEnum)reader.ReadUInt32();
			Data = reader.ReadBytes(36);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write((uint)EventMode);
			writer.Write(Data);
		}
	}

	// Possibly moves protagonist via a bezier curve?
	internal class HiruSaso : P3Target_CustomEvent
	{
		[JsonPropertyOrder(-91)]
		public byte ProtagResourceIndex { get; set; } // SHUZINKOU (likely 主人公 roughly meaning protagonist) RESID in editor

		[JsonPropertyOrder(-90)]
		public byte ProtagResourceType { get; set; }
		
		[JsonPropertyOrder(-89)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		protected override void ReadData(BinaryReader reader)
		{
			EventMode = (EventModeEnum)reader.ReadUInt32();
			ProtagResourceIndex = reader.ReadByte();
			ProtagResourceType = reader.ReadByte();
			Data = reader.ReadBytes(34);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write((uint)EventMode);
			writer.Write(ProtagResourceIndex);
			writer.Write(ProtagResourceType);
			writer.Write(Data);
		}
	}

	internal class StatChange : P3Target_CustomEvent
	{
		[JsonPropertyOrder(-91)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public ChangeTypeEnum ChangeType { get; set; }

		[JsonPropertyOrder(-90)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public ChangeStatEnum ChangeStat { get; set; }

		[JsonPropertyOrder(-89)]
		public ushort Field1A { get; set; }

		// Percentage chances for X increase?
		[JsonPropertyOrder(-88)]
		public sbyte FourtyPercentValue { get; set; }

		[JsonPropertyOrder(-87)]
		public sbyte ThirtyPercentValue { get; set; }

		[JsonPropertyOrder(-86)]
		public sbyte TwentyPercentValue { get; set; }

		[JsonPropertyOrder(-85)]
		public sbyte TenPercentValue { get; set; }

		[JsonPropertyOrder(-84)]
		public byte TargetResourceIndex { get; set; }

		[JsonPropertyOrder(-83)]
		public byte TargetResourceType { get; set; }

		[JsonPropertyOrder(-82)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		public enum ChangeTypeEnum : byte
		{
			UP = 0,
			DOWN = 1
		}

		public enum ChangeStatEnum : byte
		{
			ACADEMICS = 0, // GAKURYOKU in editor; Likely 学力 roughly meaning "scholarly ability" == Academics
			CHARM = 1, // MIRYOKU in editor; Likely 魅力 meaning Charm
			COURAGE = 2, // YUUKI in editor; Likely 勇気 meaning Courage
			TIKARA_PER = 3, // Likely 力 roughly meaning Persona Strength
			MA_PER = 4, // Persona Magic?
			TAI_PER = 5,
			SOKU_PER = 6, // Persona Speed?
			UN_PER = 7 // Likely 運 roughly meaning Persona Luck
		}

		protected override void ReadData(BinaryReader reader)
		{
			EventMode = (EventModeEnum)reader.ReadUInt32();
			ChangeType = (ChangeTypeEnum)reader.ReadByte();
			ChangeStat = (ChangeStatEnum)reader.ReadByte();
			Field1A = reader.ReadUInt16();
			FourtyPercentValue = reader.ReadSByte();
			ThirtyPercentValue = reader.ReadSByte();
			TwentyPercentValue = reader.ReadSByte();
			TenPercentValue = reader.ReadSByte();
			TargetResourceIndex = reader.ReadByte();
			TargetResourceType = reader.ReadByte();
			Data = reader.ReadBytes(26);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write((uint)EventMode);
			writer.Write((byte)ChangeType);
			writer.Write((byte)ChangeStat);
			writer.Write(Field1A);
			writer.Write(FourtyPercentValue);
			writer.Write(ThirtyPercentValue);
			writer.Write(TwentyPercentValue);
			writer.Write(TenPercentValue);
			writer.Write(TargetResourceIndex);
			writer.Write(TargetResourceType);
			writer.Write(Data);
		}
	}

	internal class MorauPre : P3Target_CustomEvent
	{
		[JsonPropertyOrder(-91)]
		public short MORAWANAI_JUMPFRAME { get; set; } // Limited 0-30000 in editor

		[JsonPropertyOrder(-90)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		protected override void ReadData(BinaryReader reader)
		{
			EventMode = (EventModeEnum)reader.ReadUInt32();
			MORAWANAI_JUMPFRAME = reader.ReadInt16();
			Data = reader.ReadBytes(34);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write((uint)EventMode);
			writer.Write(MORAWANAI_JUMPFRAME);
			writer.Write(Data);
		}
	}

	internal class AgeruPre : P3Target_CustomEvent
	{
		[JsonPropertyOrder(-91)]
		public short AGENAI_JUMP { get; set; } // Likely あげない which roughly means "Don't Give" and limited 0-30000 in editor

		[JsonPropertyOrder(-90)]
		public short SHOU_YOROKOBI_JUMP { get; set; } // Likely 小_喜び which roughly means "Small Joy" and limited 0-30000 in editor

		[JsonPropertyOrder(-89)]
		public short TYUU_YOROKOBI_JUMP { get; set; } // Likely 中_喜び which roughly means "Medium Joy" and limited 0-30000 in editor

		[JsonPropertyOrder(-88)]
		public short DAI_YOROKOBI_JUMP { get; set; } // Likely 大_喜び which roughly means "Large Joy" and limited 0-30000 in editor

		[JsonPropertyOrder(-87)]
		public byte AITE_ResourceIndex { get; set; }

		[JsonPropertyOrder(-86)]
		public byte AITE_ResourceType { get; set; }

		[JsonPropertyOrder(-85)]
		public short AITE_YOROKOBI_MOTNO { get; set; } // Limited 0-304 in editor, also read only as 0-1023?

		[JsonPropertyOrder(-84)]
		public short NEXT_MOTNO { get; set; } // Limited 0-304 in editor, also read only as 0-1023?

		[JsonPropertyOrder(-83)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public AgeruModeEnum Type { get; set; }

		[JsonPropertyOrder(-82)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		internal enum AgeruModeEnum : byte
		{
			NORMAL = 0,
			CHRISTMAS = 1
		}

		protected override void ReadData(BinaryReader reader)
		{
			EventMode = (EventModeEnum)reader.ReadUInt32();
			AGENAI_JUMP = reader.ReadInt16();
			SHOU_YOROKOBI_JUMP = reader.ReadInt16();
			TYUU_YOROKOBI_JUMP = reader.ReadInt16();
			DAI_YOROKOBI_JUMP = reader.ReadInt16();
			AITE_ResourceIndex = reader.ReadByte();
			AITE_ResourceType = reader.ReadByte();
			AITE_YOROKOBI_MOTNO = reader.ReadInt16();
			NEXT_MOTNO = reader.ReadInt16();
			Type = (AgeruModeEnum)reader.ReadByte();
			Data = reader.ReadBytes(21);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write((uint)EventMode);
			writer.Write(AGENAI_JUMP);
			writer.Write(SHOU_YOROKOBI_JUMP);
			writer.Write(TYUU_YOROKOBI_JUMP);
			writer.Write(DAI_YOROKOBI_JUMP);
			writer.Write(AITE_ResourceIndex);
			writer.Write(AITE_ResourceType);
			writer.Write(AITE_YOROKOBI_MOTNO);
			writer.Write(NEXT_MOTNO);
			writer.Write((byte)Type);
			writer.Write(Data);
		}
	}

	internal class CardEff : P3Target_CustomEvent
	{
		[JsonPropertyOrder(-91)]
		public byte ProtagResourceIndex { get; set; } // SHUZINKOU (likely 主人公 roughly meaning protagonist) RESID in editor

		[JsonPropertyOrder(-90)]
		public byte ProtagResourceType { get; set; }
		
		[JsonPropertyOrder(-89)]
		public sbyte SocialLinkID { get; set; }

		[JsonPropertyOrder(-88)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		internal enum CardTypeEnum : byte
		{
			KAIKIN = 0,
			LVUP = 1,
			REVERSE = 2,
			HAMETU = 3
		}

		protected override void ReadData(BinaryReader reader)
		{
			EventMode = (EventModeEnum)reader.ReadUInt32();
			ProtagResourceIndex = reader.ReadByte();
			ProtagResourceType = reader.ReadByte();
			SocialLinkID = reader.ReadSByte();
			Data = reader.ReadBytes(33);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write((uint)EventMode);
			writer.Write(ProtagResourceIndex);
			writer.Write(ProtagResourceType);
			writer.Write(SocialLinkID);
			writer.Write(Data);
		}
	}
}
