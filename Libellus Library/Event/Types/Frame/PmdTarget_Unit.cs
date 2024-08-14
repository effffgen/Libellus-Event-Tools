using LibellusLibrary.JSON;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types.Frame
{
	// TODO: Refactor this to use a similar Factory/Reader setup (ala the object table) so that
	// the output of UNIT frametargets don't use type discriminators and are slightly cleaner
	internal class PmdTarget_Unit : PmdTargetType
	{
		[JsonPropertyOrder(-92)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public UnitTypeEnum UnitType { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable
		[JsonPropertyOrder(-91)]
		public Unit UnitData { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

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

		protected override void ReadData(BinaryReader reader)
		{
			UnitType = (UnitTypeEnum)reader.ReadUInt32();
			UnitData = UnitType switch
			{
				UnitTypeEnum.DISP => new DisplayUnit(),
				UnitTypeEnum.POS_CHANGE_D => new SetPositionDirect(),
				UnitTypeEnum.MOTION_CHANGE => new ChangeAnimation(),
				UnitTypeEnum.ICON_DISP => new DisplayIcon(),
				UnitTypeEnum.KUBI => new RotateHead(),
				_ => new UnknownUnit()
			};
			UnitData.ReadData(reader);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write((uint)UnitType);
			UnitData.WriteData(writer);
		}
	}

	[JsonDerivedType(typeof(UnknownUnit), typeDiscriminator: "unk")]
	[JsonDerivedType(typeof(DisplayUnit), typeDiscriminator: "dsp")]
	[JsonDerivedType(typeof(SetPositionDirect), typeDiscriminator: "spd")]
	[JsonDerivedType(typeof(ChangeAnimation), typeDiscriminator: "ani")]
	[JsonDerivedType(typeof(DisplayIcon), typeDiscriminator: "dip")]
	[JsonDerivedType(typeof(RotateHead), typeDiscriminator: "kubi")]
	public class Unit
	{
		public virtual void ReadData(BinaryReader reader) => throw new InvalidOperationException();
		public virtual void WriteData(BinaryWriter writer) => throw new InvalidOperationException();
	}

	public class UnknownUnit : Unit
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

	public class DisplayUnit : Unit
	{
		[JsonPropertyOrder(-90)]
		public DispUnitEnum DisplayMode { get; set; } // "ON/OFF MODE" in editor

		[JsonPropertyOrder(-89)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>(); // Probably unused in P3, possibly used in P4?

		public enum DispUnitEnum : byte
		{
			ON = 0,
			OFF = 1
		}

		public override void ReadData(BinaryReader reader)
		{
			DisplayMode = (DispUnitEnum)reader.ReadByte();
			Data = reader.ReadBytes(35);
		}

		public override void WriteData(BinaryWriter writer)
		{
			writer.Write((byte)DisplayMode);
			writer.Write(Data);
		}
	}

	public class SetPositionDirect : Unit
	{
		[JsonPropertyOrder(-90)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public HokanTypeEnum InterpolationType { get; set; } // HOKANTYPE in editor (HOKAN/Possibly 補間 roughly means "interpolation")

		[JsonPropertyOrder(-89)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public MoveTypeEnum MOVETYPE { get; set; } // Maybe controls whether to change position at a constant rate (SPEED) or over set number of frames (LENGTH)?

		[JsonPropertyOrder(-88)]
		public short SPEED { get; set; } // Treated as index for SPEED1-SPEED100 if MOVETYPE == SPEED

		[JsonPropertyOrder(-87)]
		public float PosX { get; set; }

		[JsonPropertyOrder(-86)]
		public float PosY { get; set; }

		[JsonPropertyOrder(-85)]
		public float PosZ { get; set; }

		[JsonPropertyOrder(-84)]
		public byte LOOP { get; set; }

		[JsonPropertyOrder(-83)]
		public byte MUSI_STOP { get; set; }

		[JsonPropertyOrder(-82)]
		public byte AUTO_ROT { get; set; }

		[JsonPropertyOrder(-81)]
		public byte Field2B { get; set; }

		[JsonPropertyOrder(-80)]
		public float Pitch { get; set; } // Seemingly not possible to edit via event editor normally
		[JsonPropertyOrder(-79)]
		public float Yaw { get; set; }

		[JsonPropertyOrder(-78)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		[JsonPropertyOrder(-77)]
		public byte FootstepSoundsUse { get; set; } // Whether to play footstep SFX or not; ASIOTO_USE in editor (ASIOTO/Possibly 足音 roughly means "sound of footsteps")

		[JsonPropertyOrder(-76)]
		public byte FootstepChannelNo { get; set; } // Which channel to play footstep SFX in; ASIOTO_CHNO in editor

		[JsonPropertyOrder(-75)]
		public byte FootstepInterval { get; set; } // Number of frames(?) between footstep SFX being played; ASIOTO_KANKAKU in editor (KANKAKU/Possibly 間隔 roughly means "interval")

		[JsonPropertyOrder(-74)]
		public byte FootstepWait { get; set; } // Number of frames(?) to wait before playing any footstep SFX; ASIOTO_WAIT in editor

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

		public override void ReadData(BinaryReader reader)
		{
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
			FootstepSoundsUse = reader.ReadByte();
			FootstepChannelNo = reader.ReadByte();
			FootstepInterval = reader.ReadByte();
			FootstepWait = reader.ReadByte();
		}

		public override void WriteData(BinaryWriter writer)
		{
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
			writer.Write(FootstepSoundsUse);
			writer.Write(FootstepChannelNo);
			writer.Write(FootstepInterval);
			writer.Write(FootstepWait);
		}
	}

	public class ChangeAnimation : Unit
	{
		[JsonPropertyOrder(-90)]
		public sbyte AnimationGroup { get; set; } // Limited 0-(Maximum number of ??? in RMD - 1) in editor

		[JsonPropertyOrder(-89)]
		public sbyte AnimationIndex { get; set; } // Limited 0-(Maximum number of animations in RMD - 1) in editor or 0-9 if AnimationType == REF

		[JsonPropertyOrder(-88)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public LoopModeEnum LoopMode { get; set; }

		[JsonPropertyOrder(-87)]
		public sbyte WaitFrames { get; set; } // HOKAN in editor; Limited 0-100 in editor

		[JsonPropertyOrder(-86)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();
        
		[JsonPropertyOrder(-85)]
		public short OFFSET { get; set; } // Min. value of 0 in editor; editor incorrectly reads this as an int?

		[JsonPropertyOrder(-84)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public MotionTypeEnum AnimationType { get; set; }
        
		[JsonPropertyOrder(-83)]
		public byte MOT2USE { get; set; }
        
		[JsonPropertyOrder(-82)]
		public sbyte MOT2NO { get; set; }
        
		[JsonPropertyOrder(-81)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public LoopModeEnum LOOP2 { get; set; }
        
		[JsonPropertyOrder(-80)]
		public short HOKAN2 { get; set; } // Limited 0-100 in editor
        
		[JsonPropertyOrder(-79)]
		public short SPEED { get; set; } // Limited to 10-500 in editor
        
		[JsonPropertyOrder(-78)]
		public short SPEED2 { get; set; } // Limited to 10-500 in editor
        
		[JsonPropertyOrder(-77)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data2 { get; set; } = Array.Empty<byte>();

		public enum LoopModeEnum : byte
		{
			REPEAT = 0,
			END = 1
		}
		public enum MotionTypeEnum : byte
		{
			DIRECT = 0,
			REF = 1
		}

		public override void ReadData(BinaryReader reader)
		{
			AnimationGroup = reader.ReadSByte();
			AnimationIndex = reader.ReadSByte();
			LoopMode = (LoopModeEnum)reader.ReadByte();
			WaitFrames = reader.ReadSByte();
			Data = reader.ReadBytes(8);
			OFFSET = reader.ReadInt16();
			AnimationType = (MotionTypeEnum)reader.ReadByte();
			MOT2USE = reader.ReadByte();
			MOT2NO = reader.ReadSByte();
			LOOP2 = (LoopModeEnum)reader.ReadByte();
			HOKAN2 = reader.ReadInt16();
			SPEED = reader.ReadInt16();
			SPEED2 = reader.ReadInt16();
			Data2 = reader.ReadBytes(12);
		}

		public override void WriteData(BinaryWriter writer)
		{
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

	public class DisplayIcon : Unit
	{
		[JsonPropertyOrder(-90)]
		public ushort IconID { get; set; }

		[JsonPropertyOrder(-89)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		public override void ReadData(BinaryReader reader)
		{
			IconID = reader.ReadUInt16();
			Data = reader.ReadBytes(34);
		}

		public override void WriteData(BinaryWriter writer)
		{
			writer.Write(IconID);
			writer.Write(Data);
		}
	}
	
	public class RotateHead : Unit
	{
		[JsonPropertyOrder(-90)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public KubiModeEnum KubiMode { get; set; }
		
		[JsonPropertyOrder(-89)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public KubiSpeedEnum KubiSpeed { get; set; }

		[JsonPropertyOrder(-88)]
		public ushort Field1A { get; set; }

		[JsonPropertyOrder(-87)]
		public float KubiPitch { get; set; }

		[JsonPropertyOrder(-86)]
		public float KubiYaw { get; set; }

		[JsonPropertyOrder(-85)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = {};

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

		public override void ReadData(BinaryReader reader)
		{
			KubiMode = (KubiModeEnum)reader.ReadByte();
			KubiSpeed = (KubiSpeedEnum)reader.ReadByte();
			Field1A = reader.ReadUInt16();
			KubiPitch = reader.ReadSingle();
			KubiYaw = reader.ReadSingle();
			Data = reader.ReadBytes(24);
		}

		public override void WriteData(BinaryWriter writer)
		{
			writer.Write((byte)KubiMode);
			writer.Write((byte)KubiSpeed);
			writer.Write(Field1A);
			writer.Write(KubiPitch);
			writer.Write(KubiYaw);
			writer.Write(Data);
		}
	}
}
