using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types.Frame
{
	public class PmdFlags
	{
		[JsonPropertyOrder(-100)]
		public ushort FlagNumber { get; set; }

		[JsonPropertyOrder(-99)]
		public ushort CmpValue { get; set; }

		[JsonPropertyOrder(-98)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public UnitFlagType FlagType { get; set; }

		// GFlagType == GlobalFlagType
		[JsonPropertyOrder(-97)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public UnitGFlagType GFlagType { get; set; }

		internal void ReadData(BinaryReader reader)
		{
			FlagNumber = reader.ReadUInt16();
			CmpValue = reader.ReadUInt16();
			FlagType = (UnitFlagType)reader.ReadUInt16();
			GFlagType = (UnitGFlagType)reader.ReadUInt16();
		}

		internal void WriteData(BinaryWriter writer)
		{
			writer?.Write(FlagNumber);
			writer?.Write(CmpValue);
			writer?.Write((ushort)FlagType);
			writer?.Write((ushort)GFlagType);
		}

		public enum UnitFlagType : short
		{
			DISABLE = 0,
			LOCAL = 1,
			GLOBAL = 2,
		}

		public enum UnitGFlagType : short
		{
			EVT = 0,
			COMMU = 1,
			SYS = 2,
		}
	}
}
