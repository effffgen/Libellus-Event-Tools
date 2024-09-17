using LibellusLibrary.JSON;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types.Frame
{
	// TODO: implement different modes ala UNIT
	internal class P3Target_CustomEvent : P3TargetType
	{
		[JsonPropertyOrder(-92)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public EventModeEnum EventMode { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable
		[JsonPropertyOrder(-91)]
		public EventParam EventData { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

		// PERU == Persona?
		internal enum EventModeEnum : uint
		{
			HIRU_SASO = 0, // Likely means roughly "Invited at noon"
			BASHO2SAS = 1,
			PARA_UPDW = 2,
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

		protected override void ReadData(BinaryReader reader)
		{
			EventMode = (EventModeEnum)reader.ReadUInt32();
			EventData = EventMode switch
			{
				EventModeEnum.PARA_UPDW => new StatChange(),
				// EventModeEnum.GAMEOVER => new GameOver(),
				_ => new UnknownEvent()
			};
			EventData.ReadData(reader);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write((uint)EventMode);
			EventData.WriteData(writer);
		}
	}
	
	[JsonDerivedType(typeof(UnknownEvent), typeDiscriminator: "unkev")]
	[JsonDerivedType(typeof(StatChange), typeDiscriminator: "stch")]
	// [JsonDerivedType(typeof(GameOver), typeDiscriminator: "gamov")]
	public class EventParam
	{
		public virtual void ReadData(BinaryReader reader) => throw new InvalidOperationException();
		public virtual void WriteData(BinaryWriter writer) => throw new InvalidOperationException();
	}

	public class UnknownEvent : EventParam
	{
		[JsonPropertyOrder(-90)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		public override void ReadData(BinaryReader reader)
		{
			Data = reader.ReadBytes(36);
		}

		public override void WriteData(BinaryWriter writer)
		{
			writer.Write(Data);
		}
	}

	public class StatChange : EventParam
	{
		[JsonPropertyOrder(-90)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public ChangeTypeEnum ChangeType { get; set; }

		[JsonPropertyOrder(-89)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public ChangeStatEnum ChangeStat { get; set; }

		[JsonPropertyOrder(-88)]
		public ushort Field1A { get; set; }

		// Percentage chances for X increase?
		[JsonPropertyOrder(-87)]
		public sbyte FourtyPercentValue { get; set; }

		[JsonPropertyOrder(-86)]
		public sbyte ThirtyPercentValue { get; set; }

		[JsonPropertyOrder(-85)]
		public sbyte TwentyPercentValue { get; set; }

		[JsonPropertyOrder(-84)]
		public sbyte TenPercentValue { get; set; }

		[JsonPropertyOrder(-83)]
		public byte ResourceIndex { get; set; }

		[JsonPropertyOrder(-82)]
		public byte ResourceType { get; set; }

		[JsonPropertyOrder(-81)]
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

		public override void ReadData(BinaryReader reader)
		{
			ChangeType = (ChangeTypeEnum)reader.ReadByte();
			ChangeStat = (ChangeStatEnum)reader.ReadByte();
			Field1A = reader.ReadUInt16();
			FourtyPercentValue = reader.ReadSByte();
			ThirtyPercentValue = reader.ReadSByte();
			TwentyPercentValue = reader.ReadSByte();
			TenPercentValue = reader.ReadSByte();
			ResourceIndex = reader.ReadByte();
			ResourceType = reader.ReadByte();
			Data = reader.ReadBytes(26);
		}

		public override void WriteData(BinaryWriter writer)
		{
			writer.Write((byte)ChangeType);
			writer.Write((byte)ChangeStat);
			writer.Write(Field1A);
			writer.Write(FourtyPercentValue);
			writer.Write(ThirtyPercentValue);
			writer.Write(TwentyPercentValue);
			writer.Write(TenPercentValue);
			writer.Write(ResourceIndex);
			writer.Write(ResourceType);
			writer.Write(Data);
		}
	}
}
