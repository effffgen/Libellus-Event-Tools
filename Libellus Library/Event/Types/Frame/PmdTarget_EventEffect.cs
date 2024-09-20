using LibellusLibrary.JSON;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types.Frame
{
	internal class P3Target_EventEffect : P3TargetType, ITargetVarying
	{
		[JsonPropertyOrder(-92)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public EffectTypeEnum EffectType { get; set; }
		
		[JsonPropertyOrder(-91)]
		public sbyte BankNumber { get; set; } // Limited 0-16 in editor

		internal enum EffectTypeEnum : byte
		{
			CREATE = 0,
			MOVE_DIRECT = 1,
			DELETE = 2,
			ALPHA = 3
		}

		public PmdTargetType GetVariant(BinaryReader reader)
		{
			reader.BaseStream.Position += 18;
			return GetEventEffect((EffectTypeEnum)reader.ReadByte());
		}

		public PmdTargetType GetVariant()
		{
			return GetEventEffect(EffectType);
		}

		public static PmdTargetType GetEventEffect(EffectTypeEnum mode) => mode switch
		{
			EffectTypeEnum.CREATE => new CreateEffect(),
			EffectTypeEnum.MOVE_DIRECT => new MoveDirectEffect(),
			EffectTypeEnum.ALPHA => new AlphaEffect(),
			_ => new UnknownEffect()
		};
	}

	internal class UnknownEffect : P3Target_EventEffect
	{
		[JsonPropertyOrder(-90)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		protected override void ReadData(BinaryReader reader)
		{
			EffectType = (EffectTypeEnum)reader.ReadByte();
			BankNumber = reader.ReadSByte();
			Data = reader.ReadBytes(38);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write((byte)EffectType);
			writer.Write(BankNumber);
			writer.Write(Data);
		}
	}

	internal class CreateEffect : P3Target_EventEffect
	{
		[JsonPropertyOrder(-90)]
		public sbyte EffectNumber { get; set; }

		[JsonPropertyOrder(-89)]
		public byte Draw2D { get; set; }

		[JsonPropertyOrder(-88)]
		public Pmd_Vector3 Position { get; set; } = new();

		[JsonPropertyOrder(-87)]
		public Pmd_Vector3 Rotation { get; set; } = new(); // Roll, Yaw, Pitch

		[JsonPropertyOrder(-86)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		protected override void ReadData(BinaryReader reader)
		{
			EffectType = (EffectTypeEnum)reader.ReadByte();
			BankNumber = reader.ReadSByte();
			EffectNumber = reader.ReadSByte();
			Draw2D = reader.ReadByte();
			Position.X = reader.ReadSingle();
			Position.Y = reader.ReadSingle();
			Position.Z = reader.ReadSingle();
			Rotation.X = reader.ReadSingle();
			Rotation.Y = reader.ReadSingle();
			Rotation.Z = reader.ReadSingle();
			Data = reader.ReadBytes(12);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write((byte)EffectType);
			writer.Write(BankNumber);
			writer.Write(EffectNumber);
			writer.Write(Draw2D);
			writer.Write(Position.X);
			writer.Write(Position.Y);
			writer.Write(Position.Z);
			writer.Write(Rotation.X);
			writer.Write(Rotation.Y);
			writer.Write(Rotation.Z);
			writer.Write(Data);
		}
	}

	internal class MoveDirectEffect : P3Target_EventEffect
	{
		[JsonPropertyOrder(-90)]
		public short EffectLength { get; set; } // Limited 0-5000 in editor

		[JsonPropertyOrder(-89)]
		public Pmd_Vector3 Position { get; set; } = new();

		[JsonPropertyOrder(-88)]
		public Pmd_Vector3 Rotation { get; set; } = new(); // Roll, Yaw, Pitch

		[JsonPropertyOrder(-87)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		protected override void ReadData(BinaryReader reader)
		{
			EffectType = (EffectTypeEnum)reader.ReadByte();
			BankNumber = reader.ReadSByte();
			EffectLength = reader.ReadInt16();
			Position.X = reader.ReadSingle();
			Position.Y = reader.ReadSingle();
			Position.Z = reader.ReadSingle();
			Rotation.X = reader.ReadSingle();
			Rotation.Y = reader.ReadSingle();
			Rotation.Z = reader.ReadSingle();
			Data = reader.ReadBytes(12);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write((byte)EffectType);
			writer.Write(BankNumber);
			writer.Write(EffectLength);
			writer.Write(Position.X);
			writer.Write(Position.Y);
			writer.Write(Position.Z);
			writer.Write(Rotation.X);
			writer.Write(Rotation.Y);
			writer.Write(Rotation.Z);
			writer.Write(Data);
		}
	}

	internal class AlphaEffect : P3Target_EventEffect
	{
		[JsonPropertyOrder(-90)]
		public byte Alpha { get; set; }

		[JsonPropertyOrder(-89)]
		public byte Field17 { get; set; }

		[JsonPropertyOrder(-88)]
		public short AlphaLength { get; set; }

		[JsonPropertyOrder(-87)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		protected override void ReadData(BinaryReader reader)
		{
			EffectType = (EffectTypeEnum)reader.ReadByte();
			BankNumber = reader.ReadSByte();
			Alpha = reader.ReadByte();
			Field17 = reader.ReadByte();
			AlphaLength = reader.ReadInt16();
			Data = reader.ReadBytes(34);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write((byte)EffectType);
			writer.Write(BankNumber);
			writer.Write(Alpha);
			writer.Write(Field17);
			writer.Write(AlphaLength);
			writer.Write(Data);
		}
	}
}
