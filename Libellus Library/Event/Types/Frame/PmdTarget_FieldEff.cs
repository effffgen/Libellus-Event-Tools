using LibellusLibrary.JSON;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types.Frame
{
	// P4G appears to store this data completely differently... TODO: Figure that out!!!
	internal class P3Target_FieldEff : P3TargetType, ITargetVarying
	{
		[JsonPropertyOrder(-92)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public EffectModeEnum EffectMode { get; set; }
		[JsonPropertyOrder(-91)]
		public byte Field15 { get; set; }
		[JsonPropertyOrder(-90)]
		public byte EffResourceIndex { get; set; }
		[JsonPropertyOrder(-89)]
		public byte EffResourceType { get; set; } // Always 0x2C (44)?

		internal enum EffectModeEnum : byte
		{
			DISPONOFF = 0
		}

		public PmdTargetType GetVariant(BinaryReader reader)
		{
			reader.BaseStream.Position += 18;
			return GetFieldEff((EffectModeEnum)reader.ReadByte());
		}

		public PmdTargetType GetVariant()
		{
			return GetFieldEff(EffectMode);
		}

		public static PmdTargetType GetFieldEff(EffectModeEnum mode) => mode switch
		{
			EffectModeEnum.DISPONOFF => new DisplayFldEff(),
			_ => new UnknownFldEff()
		};
	}

	internal class UnknownFldEff : P3Target_FieldEff
	{
		[JsonPropertyOrder(-88)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		protected override void ReadData(BinaryReader reader)
		{
			EffectMode = (EffectModeEnum)reader.ReadByte();
			Field15 = reader.ReadByte();
			EffResourceIndex = reader.ReadByte();
			EffResourceType = reader.ReadByte();
			Data = reader.ReadBytes(36);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write((byte)EffectMode);
			writer.Write(Field15);
			writer.Write(EffResourceIndex);
			writer.Write(EffResourceType);
			writer.Write(Data);
		}
	}

	internal class DisplayFldEff : P3Target_FieldEff
	{
		[JsonPropertyOrder(-88)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public OnOffEnum OnOff { get; set; }

		[JsonPropertyOrder(-87)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		protected override void ReadData(BinaryReader reader)
		{
			EffectMode = (EffectModeEnum)reader.ReadByte();
			Field15 = reader.ReadByte();
			EffResourceIndex = reader.ReadByte();
			EffResourceType = reader.ReadByte();
			OnOff = (OnOffEnum)reader.ReadByte();
			Data = reader.ReadBytes(35);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write((byte)EffectMode);
			writer.Write(Field15);
			writer.Write(EffResourceIndex);
			writer.Write(EffResourceType);
			writer.Write((byte)OnOff);
			writer.Write(Data);
		}
	}
}
