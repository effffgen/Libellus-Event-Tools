using LibellusLibrary.JSON;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types.Frame
{
    // TODO: implement different modes ala UNIT
    internal class PmdTarget_CustomEvent : PmdTargetType
    {
        [JsonPropertyOrder(-92)]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EventModeEnum EventMode { get; set; }

        [JsonPropertyOrder(-91)]
        [JsonConverter(typeof(ByteArrayToHexArray))]
        public byte[] Data { get; set; }
        
        // PERU == Persona?
        internal enum EventModeEnum : uint
        {
            HIRU_SASO = 0,
            BASHO2SAS = 1,
            PARA_UPDW = 2,
            HOLYHOGO = 3,
            HANDENWA = 4,
            UWAKI_HAK = 5,
            MORAU_PRE = 6,
            AGERU_PRE = 7,
            HIDUKEMES = 8,
            CARDEFF = 9,
            GAMEOVER = 10,
            HAVEPERU = 11,
            LVUPMES = 12,
            PERUKOE = 13,
        }

        protected override void ReadData(BinaryReader reader)
        {
            EventMode = (EventModeEnum)reader.ReadUInt32();
            Data = reader.ReadBytes(36);
        }

        protected override void WriteData(BinaryWriter writer)
        {
            writer?.Write((uint)EventMode);
            writer?.Write(Data);
        }
    }
}
