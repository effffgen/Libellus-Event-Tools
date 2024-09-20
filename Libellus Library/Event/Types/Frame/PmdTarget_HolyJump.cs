using LibellusLibrary.JSON;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types.Frame
{
	// HolyJump is probably short for "Holiday Jump"?
	internal class P3Target_HolyJump : P3TargetType
	{
		// KYUJITU YAKUSOKU JUMP MENU - likely very roughly means "Weekend Holiday Promise Jump" (used for SL weekend dates)
		[JsonPropertyOrder(-92)]
		public short IsAvailableFrame { get; set; } // Go to this frame if weekend isn't busy; YAKUSOKU_DEKIRU_JUMP in editor+limited to positive values only (0x0-0x7FFF)

		[JsonPropertyOrder(-92)]
		public short NotAvailableFrame { get; set; } // Go to this frame if busy on weekend; YAKUSOKU_DEKINAI_JUMP in editor+limited to positive values only (0x0-0x7FFF)

		[JsonPropertyOrder(-90)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		protected override void ReadData(BinaryReader reader)
		{
			IsAvailableFrame = reader.ReadInt16();
			NotAvailableFrame = reader.ReadInt16();
			Data = reader.ReadBytes(36);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write(IsAvailableFrame);
			writer.Write(NotAvailableFrame);
			writer.Write(Data);
		}
	}
}
