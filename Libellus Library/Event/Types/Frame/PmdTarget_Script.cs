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
        [JsonPropertyOrder(-96)]
        [JsonConverter(typeof(ByteArrayToHexArray))]
        public byte[] Data { get; set; }
        
        [JsonPropertyOrder(-95)]
        public PmdFlags Flags { get; set; }

        [JsonPropertyOrder(-94)]
        [JsonConverter(typeof(ByteArrayToHexArray))]
        public byte[] Data2 { get; set; }

        [JsonPropertyOrder(-93)]
        public ushort ProcedureIndex { get; set; }

        [JsonPropertyOrder(-92)]
        [JsonConverter(typeof(ByteArrayToHexArray))]
        public byte[] Data3 { get; set; }

        protected override void ReadData(BinaryReader reader)
        {
            Data = reader.ReadBytes(4);
            Flags = new PmdFlags();
            Flags.ReadData(reader);
            Data2 = reader.ReadBytes(6);
            ProcedureIndex = reader.ReadUInt16();
            Data3 = reader.ReadBytes(32);
        }

        protected override void WriteData(BinaryWriter writer)
        {
            writer?.Write(Data);
            Flags.WriteData(writer);
            writer?.Write(Data2);
            writer?.Write(ProcedureIndex);
            writer?.Write(Data3);
        }
    }
}
