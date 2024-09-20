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
			MORAU_PRE = 6,
			AGERU_PRE = 7,
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
			EventModeEnum.PARA_UPDW => new StatChange(),
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
}
