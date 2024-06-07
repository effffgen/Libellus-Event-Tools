using LibellusLibrary.JSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static LibellusLibrary.Event.Types.Frame.PmdTarget_Message;

namespace LibellusLibrary.Event.Types.Frame
{
    internal class PmdTarget_Script : PmdTargetType
    {
        [JsonPropertyOrder(-92)]
        [JsonConverter(typeof(ByteArrayToHexArray))]
        public byte[] Data { get; set; }

        [JsonPropertyOrder(-91)]
        public ushort ProcedureIndex { get; set; }

        [JsonPropertyOrder(-90)]
        [JsonConverter(typeof(ByteArrayToHexArray))]
        public byte[] Data2 { get; set; } = Array.Empty<byte>();

        protected override void ReadData(BinaryReader reader)
        {
            Data = reader.ReadBytes(6);
            ProcedureIndex = reader.ReadUInt16();
            Data2 = reader.ReadBytes(32);
        }

        protected override void WriteData(BinaryWriter writer)
        {
            writer?.Write(Data);
            writer?.Write(ProcedureIndex);
            writer?.Write(Data2);
        }
    }
}
