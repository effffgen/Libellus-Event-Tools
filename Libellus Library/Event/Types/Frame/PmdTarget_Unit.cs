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
        [JsonConverter(typeof(ByteArrayToHexArray))]
        public byte[] Data { get; set; }

        internal enum UnitTypeEnum : uint
        {
            DISP = 0,
            POS_CHANGE_D = 1, // Change position directly
            MOTION_CHANGE = 2,
            POS_CHANGE = 3,   // Change position by refrence
            ROT_CHANGE = 4,
            ICON_DISP = 5,
            ROT_HEAD = 6,     // TODO: Use event editors actual name
            MODEL_ATTACH = 7,
            ALPHA_CHANGE = 8,
            SHADOW_ONOFF = 9,
            SCALE_CHANGE = 10,
        }
        
        internal enum HokanTypeEnum : byte
        {
            POS = 0,
            BEZIER = 1,
            DIRECT = 2,
        }
        
        internal enum MoveTypeEnum : byte
        {
            SPEED = 0,
            LENGTH = 1,
        }
        
        protected override void ReadData(BinaryReader reader)
        {
            UnitType = (UnitTypeEnum)reader.ReadUInt32();
            Data = reader.ReadBytes(36);
        }

        protected override void WriteData(BinaryWriter writer)
        {
            writer?.Write((uint)UnitType);
            writer?.Write(Data);
        }
    }
}
