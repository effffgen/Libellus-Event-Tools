using LibellusLibrary.JSON;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types.Frame
{
	internal class P3Target_FieldObj : P3TargetType
	{
		[JsonPropertyOrder(-92)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public ObjectModeEnum ObjectMode { get; set; }
		[JsonPropertyOrder(-91)]
		public byte Field15 { get; set; }
		[JsonPropertyOrder(-90)]
		public byte ObjResourceIndex { get; set; }
		[JsonPropertyOrder(-89)]
		public byte ObjResourceType { get; set; } // Always 0x28 (40)?

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable
		[JsonPropertyOrder(-88)]
		public FldObjParam FieldObjData { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

		internal enum ObjectModeEnum : byte
		{
			DISP = 0,
			ANIM = 1
		}

		protected override void ReadData(BinaryReader reader)
		{
			ObjectMode = (ObjectModeEnum)reader.ReadByte();
			Field15 = reader.ReadByte();
			ObjResourceIndex = reader.ReadByte();
			ObjResourceType = reader.ReadByte();
			FieldObjData = ObjectMode switch
			{
				ObjectModeEnum.DISP => new DisplayFldObj(),
				ObjectModeEnum.ANIM => new AnimateFieldObj(),
				_ => throw new InvalidOperationException()
			};
			FieldObjData.ReadData(reader);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write((byte)ObjectMode);
			writer.Write(Field15);
			writer.Write(ObjResourceIndex);
			writer.Write(ObjResourceType);
			FieldObjData.WriteData(writer);
		}
	}
	
	[JsonDerivedType(typeof(UnknownFldObj), typeDiscriminator: "unk")]
	[JsonDerivedType(typeof(DisplayFldObj), typeDiscriminator: "dsp")]
	[JsonDerivedType(typeof(AnimateFieldObj), typeDiscriminator: "ani")]
	public class FldObjParam
	{
		public virtual void ReadData(BinaryReader reader) => throw new InvalidOperationException();
		public virtual void WriteData(BinaryWriter writer) => throw new InvalidOperationException();
	}

	public class UnknownFldObj : FldObjParam
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

	public class DisplayFldObj : FldObjParam
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

	public class AnimateFieldObj : FldObjParam
	{
		[JsonPropertyOrder(-87)]
		public sbyte AnimationIndex { get; set; } // MOT NO in editor, limited by number of animations object has

		[JsonPropertyOrder(-86)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public LoopModeEnum LoopMode { get; set; }

		[JsonPropertyOrder(-85)]
		public short Hokan { get; set; } // HOKAN NO and limited 0-32767 in editor, possibly interpolation length/intensity?

		[JsonPropertyOrder(-84)]
		public short Offset { get; set; } // OFFSET NO and limited 0-32767 in editor

		[JsonPropertyOrder(-83)]
		public short Speed { get; set; } // limited 10-500 in editor

		[JsonPropertyOrder(-82)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		public override void ReadData(BinaryReader reader)
		{
			AnimationIndex = reader.ReadSByte();
			LoopMode = (LoopModeEnum)reader.ReadByte();
			Hokan = reader.ReadInt16();
			Offset = reader.ReadInt16();
			Speed = reader.ReadInt16();
			Data = reader.ReadBytes(28);
		}

		public override void WriteData(BinaryWriter writer)
		{
			writer.Write(AnimationIndex);
			writer.Write((byte)LoopMode);
			writer.Write(Hokan);
			writer.Write(Offset);
			writer.Write(Speed);
			writer.Write(Data);
		}
	}
}
