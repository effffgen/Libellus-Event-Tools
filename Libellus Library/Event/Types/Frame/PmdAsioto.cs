using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types.Frame
{
	public class PmdAsioto
	{
		[JsonPropertyOrder(-100)]
		public byte Use { get; set; } // Whether to play footstep SFX or not; ASIOTO_USE in editor (ASIOTO/Possibly 足音 roughly means "sound of footsteps")

		[JsonPropertyOrder(-99)]
		public byte ChannelNumber { get; set; } // Which channel to play footstep SFX in; ASIOTO_CHNO in editor

		[JsonPropertyOrder(-98)]
		public byte Interval { get; set; } // Number of frames(?) between footstep SFX being played; ASIOTO_KANKAKU in editor (KANKAKU/Possibly 間隔 roughly means "interval")

		[JsonPropertyOrder(-97)]
		public byte Wait { get; set; } // Number of frames(?) to wait before playing any footstep SFX; ASIOTO_WAIT in editor

		internal void ReadData(BinaryReader reader)
		{
			Use = reader.ReadByte();
			ChannelNumber = reader.ReadByte();
			Interval = reader.ReadByte();
			Wait = reader.ReadByte();
		}

		internal void WriteData(BinaryWriter writer)
		{
			writer.Write(Use);
			writer.Write(ChannelNumber);
			writer.Write(Interval);
			writer.Write(Wait);
		}
	}
}
