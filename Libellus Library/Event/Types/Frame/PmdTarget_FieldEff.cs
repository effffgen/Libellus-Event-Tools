using LibellusLibrary.JSON;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types.Frame
{
	// P4G appears to store this data completely differently... TODO: Figure that out!!!
	internal class P3Target_FieldEff : P3TargetType
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

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable
		[JsonPropertyOrder(-88)]
		public FldEffParam FieldEffData { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

		internal enum EffectModeEnum : byte
		{
			DISPONOFF = 0
		}

		protected override void ReadData(BinaryReader reader)
		{
			EffectMode = (EffectModeEnum)reader.ReadByte();
			Field15 = reader.ReadByte();
			EffResourceIndex = reader.ReadByte();
			EffResourceType = reader.ReadByte();
			FieldEffData = EffectMode switch
			{
				EffectModeEnum.DISPONOFF => new DisplayFldEff(),
				_ => new UnknownFldEff()
			};
			FieldEffData.ReadData(reader);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write((byte)EffectMode);
			writer.Write(Field15);
			writer.Write(EffResourceIndex);
			writer.Write(EffResourceType);
			FieldEffData.WriteData(writer);
		}
	}
	
	[JsonDerivedType(typeof(UnknownFldEff), typeDiscriminator: "unk")]
	[JsonDerivedType(typeof(DisplayFldEff), typeDiscriminator: "dsp")]
	public class FldEffParam
	{
		public virtual void ReadData(BinaryReader reader) => throw new InvalidOperationException();
		public virtual void WriteData(BinaryWriter writer) => throw new InvalidOperationException();
	}

	public class UnknownFldEff : FldEffParam
	{
		[JsonPropertyOrder(-87)]
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

	public class DisplayFldEff : FldEffParam
	{
		[JsonPropertyOrder(-87)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public OnOffEnum OnOff { get; set; }

		[JsonPropertyOrder(-86)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		public override void ReadData(BinaryReader reader)
		{
			OnOff = (OnOffEnum)reader.ReadByte();
			Data = reader.ReadBytes(35);
		}

		public override void WriteData(BinaryWriter writer)
		{
			writer.Write((byte)OnOff);
			writer.Write(Data);
		}
	}

}
