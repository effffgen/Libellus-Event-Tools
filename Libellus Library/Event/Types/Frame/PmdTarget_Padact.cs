using LibellusLibrary.JSON;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types.Frame
{
	internal class PmdTarget_Padact : PmdTargetType
	{
		[JsonPropertyOrder(-92)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public PadactEnum PadactMode { get; set; }

		[JsonPropertyOrder(-91)]
		public byte Field15 { get; set; } // (as in which PADACT entry is it listed under in editor) id? Uncertain

		[JsonPropertyOrder(-90)]
		public ushort Field16 { get; set; }

		[JsonPropertyOrder(-90)]
		public short RumbleDuration { get; set; } // Total frames to rumble for; Mnemonic = L; ZIKAN (likely 時間 which roughly means time)+limited to 0-3000 in editor

		[JsonPropertyOrder(-90)]
		public short RumbleStrength { get; set; } // Mnemonic = P; TUYOSA (likely 強さ which roughly means strength)+limited to 0-255 in editor

		[JsonPropertyOrder(-90)]
		public short RumbleOnFrames { get; set; } // Number of frames to rumble for; Mnemonic = ON; ON_ZIKAN+limited to 0-1000 in editor

		[JsonPropertyOrder(-90)]
		public short RumbleOffFrames { get; set; } // Number of frames to wait before rumble; Mnemonic = OFF; OFF_ZIKAN+limited to 0-1000 in editor

		[JsonPropertyOrder(-90)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		internal enum PadactEnum : byte
		{
			START = 0,
			STOP = 1
		}

		protected override void ReadData(BinaryReader reader)
		{
			PadactMode = (PadactEnum)reader.ReadByte();
			Field15 = reader.ReadByte();
			Field16 = reader.ReadUInt16();
			RumbleDuration = reader.ReadInt16();
			RumbleStrength = reader.ReadInt16();
			RumbleOnFrames = reader.ReadInt16();
			RumbleOffFrames = reader.ReadInt16();
			Data = reader.ReadBytes(28);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write((byte)PadactMode);
			writer.Write(Field15);
			writer.Write(Field16);
			writer.Write(RumbleDuration);
			writer.Write(RumbleStrength);
			writer.Write(RumbleOnFrames);
			writer.Write(RumbleOffFrames);
			writer.Write(Data);
		}
	}
}
