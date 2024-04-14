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
            KUBI = 6,
            MODEL_ATTACH = 7,
            ALPHA_CHANGE = 8,
            SHADOW_ONOFF = 9,
            SCALE_CHANGE = 10,
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
    
    /*public class Unit_SetPositionDirect
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
        public byte ChannelNumber { get; set; 
        
        [JsonPropertyOrder(-78)]
        public byte SoundInterval { get; set; }
        
        [JsonPropertyOrder(-77)]
        public byte SoundWait { get; set; }
        
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
    }*/
    
}
