using LibellusLibrary.JSON;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types.Frame
{
	internal class P3Target_Unit : P3TargetType, ITargetVarying
	{
		[JsonPropertyOrder(-92)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public UnitTypeEnum UnitType { get; set; }

		internal enum UnitTypeEnum : uint
		{
			DISP = 0,
			POS_CHANGE_D = 1, // Change position directly
			MOTION_CHANGE = 2,
			POS_CHANGE = 3,   // Change position by refrence
			ROT_CHANGE = 4,
			ICON_DISP = 5,
			KUBI = 6,
			MODEL_ATTACH = 7,
			ALPHA_CHANGE = 8,
			SHADOW_ONOFF = 9,
			SCALE_CHANGE = 10,
		}

		internal enum AccessTypeEnum : byte
		{
			DIRECT = 0,
			REF = 1
		}

		public PmdTargetType GetVariant(BinaryReader reader)
		{
			reader.BaseStream.Position += 18;
			return GetFieldEff((UnitTypeEnum)reader.ReadUInt32());
		}

		public PmdTargetType GetVariant()
		{
			return GetFieldEff(UnitType);
		}

		public static PmdTargetType GetFieldEff(UnitTypeEnum mode) => mode switch
		{
			UnitTypeEnum.DISP => new DisplayUnit(),
			UnitTypeEnum.POS_CHANGE_D => new SetPositionDirect(),
			UnitTypeEnum.MOTION_CHANGE => new ChangeAnimation(),
			UnitTypeEnum.POS_CHANGE => new ChangePosition(),
			UnitTypeEnum.ROT_CHANGE => new ChangeRotation(),
			UnitTypeEnum.ICON_DISP => new DisplayIcon(),
			UnitTypeEnum.KUBI => new RotateHead(),
			UnitTypeEnum.MODEL_ATTACH => new ModelAttach(),
			UnitTypeEnum.ALPHA_CHANGE => new AlphaChange(),
			UnitTypeEnum.SHADOW_ONOFF => new ShadowToggle(),
			UnitTypeEnum.SCALE_CHANGE => new ScaleChange(),
			_ => new UnknownUnit()
		};
	}

	internal class UnknownUnit : P3Target_Unit
	{
		[JsonPropertyOrder(-91)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		protected override void ReadData(BinaryReader reader)
		{
			UnitType = (UnitTypeEnum)reader.ReadUInt32();
			Data = reader.ReadBytes(36);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write((uint)UnitType);
			writer.Write(Data);
		}
	}

	internal class DisplayUnit : P3Target_Unit
	{
		[JsonPropertyOrder(-91)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public OnOffEnum DisplayMode { get; set; } // "ON/OFF MODE" in editor

		[JsonPropertyOrder(-90)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>(); // Probably unused in P3, possibly used in P4?

		protected override void ReadData(BinaryReader reader)
		{
			UnitType = (UnitTypeEnum)reader.ReadUInt32();
			DisplayMode = (OnOffEnum)reader.ReadByte();
			Data = reader.ReadBytes(35);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write((uint)UnitType);
			writer.Write((byte)DisplayMode);
			writer.Write(Data);
		}
	}

	internal class SetPositionDirect : P3Target_Unit
	{
		[JsonPropertyOrder(-91)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public HokanTypeEnum InterpolationType { get; set; } // HOKANTYPE in editor (HOKAN/Possibly 補間 roughly means "interpolation")

		[JsonPropertyOrder(-90)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public MoveTypeEnum MOVETYPE { get; set; } // Maybe controls whether to change position at a constant rate (SPEED) or over set number of frames (LENGTH)?

		[JsonPropertyOrder(-89)]
		public short SPEED { get; set; } // Treated as index for SPEED1-SPEED100 if MOVETYPE == SPEED

		[JsonPropertyOrder(-88)]
		public float PosX { get; set; }

		[JsonPropertyOrder(-87)]
		public float PosY { get; set; }

		[JsonPropertyOrder(-86)]
		public float PosZ { get; set; }

		[JsonPropertyOrder(-85)]
		public byte LOOP { get; set; }

		[JsonPropertyOrder(-84)]
		public byte MUSI_STOP { get; set; }

		[JsonPropertyOrder(-83)]
		public byte AUTO_ROT { get; set; }

		[JsonPropertyOrder(-82)]
		public byte Field2B { get; set; }

		[JsonPropertyOrder(-81)]
		public float Pitch { get; set; } // Seemingly not possible to edit via event editor normally

		[JsonPropertyOrder(-80)]
		public float Yaw { get; set; }

		[JsonPropertyOrder(-79)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		[JsonPropertyOrder(-78)]
		public PmdAsioto FootstepSounds { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

		public enum HokanTypeEnum : byte
		{
			POS = 0,
			BEZIER = 1,
			DIRECT = 2,
		}

		public enum MoveTypeEnum : byte
		{
			SPEED = 0,
			LENGTH = 1,
		}

		protected override void ReadData(BinaryReader reader)
		{
			UnitType = (UnitTypeEnum)reader.ReadUInt32();
			InterpolationType = (HokanTypeEnum)reader.ReadByte();
			MOVETYPE = (MoveTypeEnum)reader.ReadByte();
			SPEED = reader.ReadInt16();
			PosX = reader.ReadSingle();
			PosY = reader.ReadSingle();
			PosZ = reader.ReadSingle();
			LOOP = reader.ReadByte();
			MUSI_STOP = reader.ReadByte();
			AUTO_ROT = reader.ReadByte();
			Field2B = reader.ReadByte();
			Pitch = reader.ReadSingle();
			Yaw = reader.ReadSingle();
			Data = reader.ReadBytes(4);
			FootstepSounds = new();
			FootstepSounds.ReadData(reader);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write((uint)UnitType);
			writer.Write((byte)InterpolationType);
			writer.Write((byte)MOVETYPE);
			writer.Write(SPEED);
			writer.Write(PosX);
			writer.Write(PosY);
			writer.Write(PosZ);
			writer.Write(LOOP);
			writer.Write(MUSI_STOP);
			writer.Write(AUTO_ROT);
			writer.Write(Field2B);
			writer.Write(Pitch);
			writer.Write(Yaw);
			writer.Write(Data);
			FootstepSounds.WriteData(writer);
		}
	}

	internal class ChangeAnimation : P3Target_Unit
	{
		[JsonPropertyOrder(-91)]
		public sbyte AnimationGroup { get; set; } // Limited 0-(Maximum number of ??? in RMD - 1) in editor

		[JsonPropertyOrder(-90)]
		public sbyte AnimationIndex { get; set; } // Limited 0-(Maximum number of animations in RMD - 1) in editor or 0-9 if AnimationType == REF

		[JsonPropertyOrder(-89)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public LoopModeEnum LoopMode { get; set; }

		[JsonPropertyOrder(-88)]
		public sbyte WaitFrames { get; set; } // HOKAN in editor; Limited 0-100 in editor

		[JsonPropertyOrder(-87)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		[JsonPropertyOrder(-86)]
		public short OFFSET { get; set; } // Min. value of 0 in editor; editor incorrectly reads this as an int?

		[JsonPropertyOrder(-85)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public AccessTypeEnum AnimationType { get; set; }

		[JsonPropertyOrder(-84)]
		public byte MOT2USE { get; set; }

		[JsonPropertyOrder(-83)]
		public sbyte MOT2NO { get; set; }

		[JsonPropertyOrder(-82)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public LoopModeEnum LOOP2 { get; set; }

		[JsonPropertyOrder(-81)]
		public short HOKAN2 { get; set; } // Limited 0-100 in editor

		[JsonPropertyOrder(-80)]
		public short SPEED { get; set; } // Limited to 10-500 in editor

		[JsonPropertyOrder(-79)]
		public short SPEED2 { get; set; } // Limited to 10-500 in editor

		[JsonPropertyOrder(-78)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data2 { get; set; } = Array.Empty<byte>();

		protected override void ReadData(BinaryReader reader)
		{
			UnitType = (UnitTypeEnum)reader.ReadUInt32();
			AnimationGroup = reader.ReadSByte();
			AnimationIndex = reader.ReadSByte();
			LoopMode = (LoopModeEnum)reader.ReadByte();
			WaitFrames = reader.ReadSByte();
			Data = reader.ReadBytes(8);
			OFFSET = reader.ReadInt16();
			AnimationType = (AccessTypeEnum)reader.ReadByte();
			MOT2USE = reader.ReadByte();
			MOT2NO = reader.ReadSByte();
			LOOP2 = (LoopModeEnum)reader.ReadByte();
			HOKAN2 = reader.ReadInt16();
			SPEED = reader.ReadInt16();
			SPEED2 = reader.ReadInt16();
			Data2 = reader.ReadBytes(12);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write((uint)UnitType);
			writer.Write(AnimationGroup);
			writer.Write(AnimationIndex);
			writer.Write((byte)LoopMode);
			writer.Write(WaitFrames);
			writer.Write(Data);
			writer.Write(OFFSET);
			writer.Write((byte)AnimationType);
			writer.Write(MOT2USE);
			writer.Write(MOT2NO);
			writer.Write((byte)LOOP2);
			writer.Write(HOKAN2);
			writer.Write(SPEED);
			writer.Write(SPEED2);
			writer.Write(Data2);
		}
	}

	internal class ChangePosition : P3Target_Unit
	{
		[JsonPropertyOrder(-91)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public PosChangeEnum ChangeType { get; set; }

		[JsonPropertyOrder(-90)]
		public sbyte SPEED { get; set; } // Limited 0x0-0x63 (SPEED1-SPEED100) in editor

		// ResourceIndex and ResourceType are both listed as "ResID" in editor menu; see ..\Object\PmdObject_SLight.cs for details
		// Seemingly can only be changed if a value is already set for given target, otherwise not possible to alter values in event editor
		[JsonPropertyOrder(-89)]
		public byte TargetResourceIndex { get; set; }

		[JsonPropertyOrder(-88)]
		public byte TargetResourceType { get; set; }

		[JsonPropertyOrder(-87)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		public enum PosChangeEnum : byte
		{
			DIRECT = 0,
			MOVE = 1,
			PATH = 2
		}
		protected override void ReadData(BinaryReader reader)
		{
			UnitType = (UnitTypeEnum)reader.ReadUInt32();
			ChangeType = (PosChangeEnum)reader.ReadByte();
			SPEED = reader.ReadSByte();
			TargetResourceIndex = reader.ReadByte();
			TargetResourceType = reader.ReadByte();
			Data = reader.ReadBytes(32);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write((uint)UnitType);
			writer.Write((byte)ChangeType);
			writer.Write(SPEED);
			writer.Write(TargetResourceIndex);
			writer.Write(TargetResourceType);
			writer.Write(Data);
		}
	}

	internal class ChangeRotation : P3Target_Unit
	{
		[JsonPropertyOrder(-91)]
		public float Yaw { get; set; } // ROT Y in editor; editor also lists a "ROT X" option which doesn't appear to be saved or read at all (though it does work)

		[JsonPropertyOrder(-90)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		[JsonPropertyOrder(-89)]
		public short RotationLength { get; set; } // ROT LENGTH in editor

		[JsonPropertyOrder(-88)]
		public ushort Field2A { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		[JsonPropertyOrder(-87)]
		public PmdAsioto FootstepSounds { get; set; } // P4G stores this at the last 4 bytes... ugh
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

		[JsonPropertyOrder(-86)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data2 { get; set; } = Array.Empty<byte>();

		protected override void ReadData(BinaryReader reader)
		{
			UnitType = (UnitTypeEnum)reader.ReadUInt32();
			Yaw = reader.ReadSingle();
			Data = reader.ReadBytes(12);
			RotationLength = reader.ReadInt16();
			Field2A = reader.ReadUInt16();
			FootstepSounds = new();
			FootstepSounds.ReadData(reader);
			Data2 = reader.ReadBytes(12);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write((uint)UnitType);
			writer.Write(Yaw);
			writer.Write(Data);
			writer.Write(RotationLength);
			writer.Write(Field2A);
			FootstepSounds.WriteData(writer);
			writer.Write(Data2);
		}
	}

	internal class DisplayIcon : P3Target_Unit
	{
		[JsonPropertyOrder(-91)]
		public sbyte IconIndex { get; set; }

		[JsonPropertyOrder(-90)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public AccessTypeEnum IndexType { get; set; }

		[JsonPropertyOrder(-89)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		protected override void ReadData(BinaryReader reader)
		{
			UnitType = (UnitTypeEnum)reader.ReadUInt32();
			IconIndex = reader.ReadSByte();
			IndexType = (AccessTypeEnum)reader.ReadByte();
			Data = reader.ReadBytes(34);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write((uint)UnitType);
			writer.Write(IconIndex);
			writer.Write((byte)IndexType);
			writer.Write(Data);
		}
	}

	internal class RotateHead : P3Target_Unit
	{
		[JsonPropertyOrder(-91)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public KubiModeEnum KubiMode { get; set; } // SYORI in editor
		
		[JsonPropertyOrder(-90)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public KubiSpeedEnum KubiSpeed { get; set; }

		[JsonPropertyOrder(-89)]
		public ushort Field1A { get; set; }

		[JsonPropertyOrder(-88)]
		public float KubiPitch { get; set; }

		[JsonPropertyOrder(-87)]
		public float KubiYaw { get; set; }

		[JsonPropertyOrder(-86)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		public enum KubiModeEnum : byte
		{
			MOVE = 0,
			RESET = 1,
		}

		public enum KubiSpeedEnum : byte
		{
			NORMAL = 0,
			FAST = 1,
		}

		protected override void ReadData(BinaryReader reader)
		{
			UnitType = (UnitTypeEnum)reader.ReadUInt32();
			KubiMode = (KubiModeEnum)reader.ReadByte();
			KubiSpeed = (KubiSpeedEnum)reader.ReadByte();
			Field1A = reader.ReadUInt16();
			KubiPitch = reader.ReadSingle();
			KubiYaw = reader.ReadSingle();
			Data = reader.ReadBytes(24);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write((uint)UnitType);
			writer.Write((byte)KubiMode);
			writer.Write((byte)KubiSpeed);
			writer.Write(Field1A);
			writer.Write(KubiPitch);
			writer.Write(KubiYaw);
			writer.Write(Data);
		}
	}

	internal class ShadowToggle : P3Target_Unit
	{
		[JsonPropertyOrder(-91)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public OnOffEnum ShadowMode { get; set; }
		
		[JsonPropertyOrder(-90)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>(); // P4G stores data within here

		protected override void ReadData(BinaryReader reader)
		{
			UnitType = (UnitTypeEnum)reader.ReadUInt32();
			ShadowMode = (OnOffEnum)reader.ReadByte();
			Data = reader.ReadBytes(35);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write((uint)UnitType);
			writer.Write((byte)ShadowMode);
			writer.Write(Data);
		}
	}

	internal class ModelAttach : P3Target_Unit
	{
		[JsonPropertyOrder(-91)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public AttachTypeEnum AttachType { get; set; }

		[JsonPropertyOrder(-90)]
		public sbyte BankNumber { get; set; }

		[JsonPropertyOrder(-89)]
		public sbyte ModelIndex { get; set; }

		[JsonPropertyOrder(-88)]
		public byte Field1B { get; set; }

		[JsonPropertyOrder(-87)]
		public int MAXPOS_ID { get; set; } // 3DMAXPOS ID & limited 0-65535 in editor

		[JsonPropertyOrder(-86)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		public enum AttachTypeEnum : byte
		{
			ATTACH = 0,
			DETACH = 1,
		}

		protected override void ReadData(BinaryReader reader)
		{
			UnitType = (UnitTypeEnum)reader.ReadUInt32();
			AttachType = (AttachTypeEnum)reader.ReadByte();
			BankNumber = reader.ReadSByte();
			ModelIndex = reader.ReadSByte();
			Field1B = reader.ReadByte();
			MAXPOS_ID = reader.ReadInt32();
			Data = reader.ReadBytes(28);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write((uint)UnitType);
			writer.Write((sbyte)AttachType);
			writer.Write(BankNumber);
			writer.Write(ModelIndex);
			writer.Write(Field1B);
			writer.Write(MAXPOS_ID);
			writer.Write(Data);
		}
	}

	internal class AlphaChange : P3Target_Unit
	{
		[JsonPropertyOrder(-91)]
		public short ChangeLength { get; set; } // limited 0-3000 in editor
		
		[JsonPropertyOrder(-90)]
		public ushort Field1A { get; set; }

		[JsonPropertyOrder(-89)]
		public byte Alpha { get; set; }

		[JsonPropertyOrder(-88)]
		public byte IsSpecial { get; set; } // TOKUSHU (likely 特殊 roughly meaning special) ALPHA+TRUE/FALSE in editor

		[JsonPropertyOrder(-87)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		protected override void ReadData(BinaryReader reader)
		{
			UnitType = (UnitTypeEnum)reader.ReadUInt32();
			ChangeLength = reader.ReadInt16();
			Field1A = reader.ReadUInt16();
			Alpha = reader.ReadByte();
			IsSpecial = reader.ReadByte();
			Data = reader.ReadBytes(30);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write((uint)UnitType);
			writer.Write(ChangeLength);
			writer.Write(Field1A);
			writer.Write(Alpha);
			writer.Write(IsSpecial);
			writer.Write(Data);
		}
	}

	internal class ScaleChange : P3Target_Unit
	{
		[JsonPropertyOrder(-91)]
		public short ChangeLength { get; set; } // limited 0-3000 in editor

		[JsonPropertyOrder(-90)]
		public ushort Field1A { get; set; }

		[JsonPropertyOrder(-89)]
		public float Scale { get; set; } // limited 0.0-100.0 in editor

		[JsonPropertyOrder(-88)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		protected override void ReadData(BinaryReader reader)
		{
			UnitType = (UnitTypeEnum)reader.ReadUInt32();
			ChangeLength = reader.ReadInt16();
			Field1A = reader.ReadUInt16();
			Scale = reader.ReadSingle();
			Data = reader.ReadBytes(28);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write((uint)UnitType);
			writer.Write(ChangeLength);
			writer.Write(Field1A);
			writer.Write(Scale);
			writer.Write(Data);
		}
	}

	public enum OnOffEnum : byte
	{
		ON = 0,
		OFF = 1
	}

	public enum LoopModeEnum : byte
	{
		REPEAT = 0,
		END = 1
	}
}
