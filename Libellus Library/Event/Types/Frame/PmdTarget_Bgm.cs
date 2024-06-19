using LibellusLibrary.JSON;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types.Frame
{
	internal class PmdTarget_Bgm : PmdTargetType
	{
		[JsonPropertyOrder(-92)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public MusicFadeEnum MusicFade { get; set; }

		[JsonPropertyOrder(-91)]
		public ushort MusicIndex { get; set; }

		[JsonPropertyOrder(-90)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		// This is only for certain accurate in P3; P4G removes these from the event editor menu,
		// however the corresponding values still seem to be used? TODO: Figure that out
		internal enum MusicFadeEnum : ushort
		{
			PLAY = 0,
			FADEIN = 1,
			FADEOUT = 2,
			TRANS = 3,
			VOLDOWN = 4,
			VOLUP = 5,
			ALLSTOP = 6,
		}

		protected override void ReadData(BinaryReader reader)
		{
			MusicFade = (MusicFadeEnum)reader.ReadUInt16();
			MusicIndex = reader.ReadUInt16();
			Data = reader.ReadBytes(36);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write((ushort)MusicFade);
			writer.Write(MusicIndex);
			writer.Write(Data);
		}
	}
}
