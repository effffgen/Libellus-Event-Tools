using LibellusLibrary.JSON;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types.Frame
{
	internal class P3Target_FieldObj : P3TargetType, ITargetVarying
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

		internal enum ObjectModeEnum : byte
		{
			DISP = 0,
			ANIM = 1
		}

		public PmdTargetType GetVariant(BinaryReader reader)
		{
			reader.BaseStream.Position += 18;
			return GetFieldObj((ObjectModeEnum)reader.ReadByte());
		}

		public PmdTargetType GetVariant()
		{
			return GetFieldObj(ObjectMode);
		}

		public static PmdTargetType GetFieldObj(ObjectModeEnum mode) => mode switch
		{
			ObjectModeEnum.DISP => new DisplayFldObj(),
			ObjectModeEnum.ANIM => new AnimateFieldObj(),
			_ => new UnknownFldObj()
		};
	}

	internal class UnknownFldObj : P3Target_FieldObj
	{
		[JsonPropertyOrder(-88)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		protected override void ReadData(BinaryReader reader)
		{
			ObjectMode = (ObjectModeEnum)reader.ReadByte();
			Field15 = reader.ReadByte();
			ObjResourceIndex = reader.ReadByte();
			ObjResourceType = reader.ReadByte();
			Data = reader.ReadBytes(36);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write((byte)ObjectMode);
			writer.Write(Field15);
			writer.Write(ObjResourceIndex);
			writer.Write(ObjResourceType);
			writer.Write(Data);
		}
	}

	internal class DisplayFldObj : P3Target_FieldObj
	{
		[JsonPropertyOrder(-88)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public OnOffEnum OnOff { get; set; }

		[JsonPropertyOrder(-87)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		protected override void ReadData(BinaryReader reader)
		{
			ObjectMode = (ObjectModeEnum)reader.ReadByte();
			Field15 = reader.ReadByte();
			ObjResourceIndex = reader.ReadByte();
			ObjResourceType = reader.ReadByte();
			OnOff = (OnOffEnum)reader.ReadByte();
			Data = reader.ReadBytes(35);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write((byte)ObjectMode);
			writer.Write(Field15);
			writer.Write(ObjResourceIndex);
			writer.Write(ObjResourceType);
			writer.Write((byte)OnOff);
			writer.Write(Data);
		}
	}

	internal class AnimateFieldObj : P3Target_FieldObj
	{
		[JsonPropertyOrder(-88)]
		public sbyte AnimationIndex { get; set; } // MOT NO in editor, limited by number of animations object has

		[JsonPropertyOrder(-87)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public LoopModeEnum LoopMode { get; set; }

		[JsonPropertyOrder(-86)]
		public short Hokan { get; set; } // HOKAN NO and limited 0-32767 in editor, possibly interpolation length/intensity?

		[JsonPropertyOrder(-85)]
		public short Offset { get; set; } // OFFSET NO and limited 0-32767 in editor

		[JsonPropertyOrder(-84)]
		public short Speed { get; set; } // limited 10-500 in editor

		[JsonPropertyOrder(-83)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		protected override void ReadData(BinaryReader reader)
		{
			ObjectMode = (ObjectModeEnum)reader.ReadByte();
			Field15 = reader.ReadByte();
			ObjResourceIndex = reader.ReadByte();
			ObjResourceType = reader.ReadByte();
			AnimationIndex = reader.ReadSByte();
			LoopMode = (LoopModeEnum)reader.ReadByte();
			Hokan = reader.ReadInt16();
			Offset = reader.ReadInt16();
			Speed = reader.ReadInt16();
			Data = reader.ReadBytes(28);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write((byte)ObjectMode);
			writer.Write(Field15);
			writer.Write(ObjResourceIndex);
			writer.Write(ObjResourceType);
			writer.Write(AnimationIndex);
			writer.Write((byte)LoopMode);
			writer.Write(Hokan);
			writer.Write(Offset);
			writer.Write(Speed);
			writer.Write(Data);
		}
	}
}
