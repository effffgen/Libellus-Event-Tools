using LibellusLibrary.JSON;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types.Frame
{
    internal class PmdTarget_Unit : PmdTargetType
    {
        [JsonPropertyOrder(-92)]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UnitTypeEnum UnitType { get; set; }

        [JsonPropertyOrder(-91)]
        public Unit UnitData { get; set; }

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
            writer?.Write((uint)UnitType);
            UnitData.WriteData(writer);
        }
    }

    [JsonDerivedType(typeof(UnknownUnit))]
    [JsonDerivedType(typeof(SetPositionDirect))]
    [JsonDerivedType(typeof(ChangeAnimation))]
    [JsonDerivedType(typeof(DisplayIcon))]
    [JsonDerivedType(typeof(RotateHead))]
    public class Unit
    {
        public virtual void ReadData(BinaryReader reader) => throw new InvalidOperationException();
        public virtual void WriteData(BinaryWriter writer) => throw new InvalidOperationException();
    }

    public class UnknownUnit : Unit
    {
        [JsonPropertyOrder(-90)]
        [JsonConverter(typeof(ByteArrayToHexArray))]
        public byte[] Data { get; set; }

        public override void ReadData(BinaryReader reader)
        {
            Data = reader.ReadBytes(36);
        }

        public override void WriteData(BinaryWriter writer)
        {
            writer?.Write(Data);
        }
    }

    public class SetPositionDirect : Unit
    {
        [JsonPropertyOrder(-90)]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public HokanTypeEnum HOKANTYPE { get; set; }

        [JsonPropertyOrder(-89)]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MoveTypeEnum MOVETYPE { get; set; }

        [JsonPropertyOrder(-88)]
        public short SPEED { get; set; }

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
        [JsonConverter(typeof(ByteArrayToHexArray))]
        public byte[] Data { get; set; }

        [JsonPropertyOrder(-80)]
        public byte PlaySound { get; set; }

        [JsonPropertyOrder(-79)]
        public byte ChannelNumber { get; set; }

        [JsonPropertyOrder(-78)]
        public byte SoundInterval { get; set; }

        [JsonPropertyOrder(-77)]
        public byte SoundWait { get; set; }

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
            HOKANTYPE = (HokanTypeEnum)reader.ReadByte();
            MOVETYPE = (MoveTypeEnum)reader.ReadByte();
            SPEED = reader.ReadInt16();
            PosX = reader.ReadSingle();
            PosY = reader.ReadSingle();
            PosZ = reader.ReadSingle();
            LOOP = reader.ReadByte();
            MUSI_STOP = reader.ReadByte();
            AUTO_ROT = reader.ReadByte();
            Data = reader.ReadBytes(13);
            PlaySound = reader.ReadByte();
            ChannelNumber = reader.ReadByte();
            SoundInterval = reader.ReadByte();
            SoundWait = reader.ReadByte();
        }

        public override void WriteData(BinaryWriter writer)
        {
            writer?.Write((byte)HOKANTYPE);
            writer?.Write((byte)MOVETYPE);
            writer?.Write(SPEED);
            writer?.Write(PosX);
            writer?.Write(PosY);
            writer?.Write(PosZ);
            writer?.Write(LOOP);
            writer?.Write(MUSI_STOP);
            writer?.Write(AUTO_ROT);
            writer?.Write(Data);
            writer?.Write(PlaySound);
            writer?.Write(ChannelNumber);
            writer?.Write(SoundInterval);
            writer?.Write(SoundWait);
        }
    }

    public class ChangeAnimation : Unit
    {
        [JsonPropertyOrder(-90)]
        public byte Field18 { get; set; }

        [JsonPropertyOrder(-89)]
        public byte AnimationIndex { get; set; }

        [JsonPropertyOrder(-88)]
        public byte Field1A { get; set; }

        [JsonPropertyOrder(-87)]
        public byte WaitFrames { get; set; }

        [JsonPropertyOrder(-86)]
        [JsonConverter(typeof(ByteArrayToHexArray))]
        public byte[] Data { get; set; }

        public override void ReadData(BinaryReader reader)
        {
            Field18 = reader.ReadByte();
            AnimationIndex = reader.ReadByte();
            Field1A = reader.ReadByte();
            WaitFrames = reader.ReadByte();
            Data = reader.ReadBytes(32);
        }

        public override void WriteData(BinaryWriter writer)
        {
            writer?.Write(Field18);
            writer?.Write(AnimationIndex);
            writer?.Write(Field1A);
            writer?.Write(WaitFrames);
            writer?.Write(Data);
        }
    }

    public class DisplayIcon : Unit
    {
        [JsonPropertyOrder(-90)]
        public ushort IconID { get; set; }

        [JsonPropertyOrder(-89)]
        [JsonConverter(typeof(ByteArrayToHexArray))]
        public byte[] Data { get; set; }

        public override void ReadData(BinaryReader reader)
        {
            IconID = reader.ReadUInt16();
            Data = reader.ReadBytes(34);
        }

        public override void WriteData(BinaryWriter writer)
        {
            writer?.Write(IconID);
            writer?.Write(Data);
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
        public byte[] Data { get; set; }

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
            writer?.Write((byte)KubiMode);
            writer?.Write((byte)KubiSpeed);
            writer?.Write(Field1A);
            writer?.Write(KubiPitch);
            writer?.Write(KubiYaw);
            writer?.Write(Data);
        }
    }
}
