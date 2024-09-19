using LibellusLibrary.JSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LibellusLibrary.Event.Types.Frame
{
	internal class PmdTarget_Bgm : PmdTargetType
	{
        [JsonPropertyOrder(-96)]
        [JsonConverter(typeof(ByteArrayToHexArray))]
        public byte[] Data { get; set; }

        [JsonPropertyOrder(-95)]
        public ushort Fade { get; set; }

        [JsonPropertyOrder(-94)]
        public ushort Id { get; set; }

        [JsonPropertyOrder(-93)]
        [JsonConverter(typeof(ByteArrayToHexArray))]
        public byte[] Data2 { get; set; }

        protected override void ReadData(BinaryReader reader)
		{
            Data = reader.ReadBytes(12);
            Fade = reader.ReadUInt16();
            Id = reader.ReadUInt16();
            Data2 = reader.ReadBytes(36);
		}

		protected override void WriteData(BinaryWriter writer)
		{
            writer?.Write(Data);
            writer?.Write(Fade);
            writer?.Write(Id);
            writer?.Write(Data2);
		}
	}
}
