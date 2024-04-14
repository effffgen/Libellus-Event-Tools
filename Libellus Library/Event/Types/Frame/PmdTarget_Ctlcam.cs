using LibellusLibrary.JSON;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types.Frame
{
    internal class PmdTarget_Ctlcam : PmdTargetType
    {
        [JsonPropertyOrder(-92)]
        public ushort Field14 { get; set; }
        
        [JsonPropertyOrder(-91)]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CameraModeEnum CameraMode { get; set; }

        [JsonPropertyOrder(-90)]
        public uint Field18 { get; set; }
        
        [JsonPropertyOrder(-89)]
        public float CameraX { get; set; }
        
        [JsonPropertyOrder(-88)]
        public float CameraY { get; set; }
        
        [JsonPropertyOrder(-87)]
        public float CameraZ { get; set; }
        
        [JsonPropertyOrder(-86)]
        public float CameraPitch { get; set; }
        
        [JsonPropertyOrder(-85)]
        public float CameraYaw { get; set; }
        
        [JsonPropertyOrder(-84)]
        public float CameraFOV { get; set; }

        [JsonPropertyOrder(-83)]
        [JsonConverter(typeof(ByteArrayToHexArray))]
        public byte[] Data { get; set; }

        internal enum CameraModeEnum : ushort
        {
            MOVE_DIRECT = 0,
            ACTIVE = 1,
        }

        protected override void ReadData(BinaryReader reader)
        {
            Field14 = reader.ReadUInt16();
            CameraMode = (CameraModeEnum)reader.ReadUInt16();
            Field18 = reader.ReadUInt32();
            CameraX = reader.ReadSingle();
            CameraY = reader.ReadSingle();
            CameraZ = reader.ReadSingle();
            CameraPitch = reader.ReadSingle();
            CameraYaw = reader.ReadSingle();
            CameraFOV = reader.ReadSingle();
            Data = reader.ReadBytes(8);
        }

        protected override void WriteData(BinaryWriter writer)
        {
            writer?.Write((ushort)Field14);
            writer?.Write((ushort)CameraMode);
            writer?.Write((uint)Field18);
            writer?.Write((float)CameraX);
            writer?.Write((float)CameraY);
            writer?.Write((float)CameraZ);
            writer?.Write((float)CameraPitch);
            writer?.Write((float)CameraYaw);
            writer?.Write((float)CameraFOV);
            writer?.Write(Data);
        }
    }
}
