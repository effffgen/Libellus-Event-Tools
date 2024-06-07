using static LibellusLibrary.Event.Types.Frame.PmdFrameFactory;
using LibellusLibrary.JSON;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types.Object
{
	public class PmdObject
	{
		[JsonPropertyOrder(-100)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public PmdTargetTypeID ObjectID { get; set; }
		
		[JsonPropertyOrder(-99)]
		public byte SlotOrID_UNK01 { get; set; }
		[JsonPropertyOrder(-98)]
		public short UNK02 { get; set; }
		[JsonPropertyOrder(-97)]
		public short UNK04 { get; set; }
		[JsonPropertyOrder(-96)]
		public sbyte UNK06 { get; set; }
		
		[JsonPropertyOrder(-95)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
        public byte[] Data { get; set; } = Array.Empty<byte>();
		
		public void ReadObject(BinaryReader reader)
		{
			ObjectID = (PmdTargetTypeID)reader.ReadByte();
			SlotOrID_UNK01 = reader.ReadByte();
			UNK02 = reader.ReadInt16();
			UNK04 = reader.ReadInt16();
			UNK06 = reader.ReadSByte();
			Data = reader.ReadBytes(13);
		}

		public void WriteObject(BinaryWriter writer)
		{ 
			writer.Write((byte)ObjectID);
			writer.Write(SlotOrID_UNK01);
			writer.Write(UNK02);
			writer.Write(UNK04);
			writer.Write(UNK06);
			writer.Write(Data);
		}
	}
}