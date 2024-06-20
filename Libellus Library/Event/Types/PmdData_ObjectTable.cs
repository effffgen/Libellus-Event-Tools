using static LibellusLibrary.Event.Types.Frame.PmdFrameFactory;
using LibellusLibrary.JSON;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types
{
	internal class PmdData_ObjectTable : PmdDataType, ITypeCreator
	{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		[JsonInclude]
		public List<Pmd_ObjectDef> Objects { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

		public PmdDataType? CreateType(uint version)
		{
			return version switch
			{
				12 => new PmdData_ObjectTable(),
				_ => new PmdData_RawData(),
			};
		}

		public PmdDataType? ReadType(BinaryReader reader, uint version, List<PmdTypeID> typeIDs, PmdTypeFactory typeFactory)
		{
			long OriginalPos = reader.BaseStream.Position;

			reader.BaseStream.Position = OriginalPos + 0x8;
			uint count = reader.ReadUInt32();

			reader.BaseStream.Position = OriginalPos + 0xC;
			reader.BaseStream.Position = (long)reader.ReadUInt32();

			Objects = new();
			for (int i = 0; i < count; i++)
			{
				Pmd_ObjectDef currentObject = new Pmd_ObjectDef();
				currentObject.ReadObject(reader);
				Objects.Add(currentObject);
			}

			reader.BaseStream.Position = OriginalPos;
			return this;
		}

		internal override void SaveData(PmdBuilder builder, BinaryWriter writer)
		{
			foreach(Pmd_ObjectDef obj in Objects)
			{
				obj.WriteObject(writer);
			}
		}
		internal override int GetCount() => Objects.Count;
		internal override int GetSize() => 0x14;
	}
	
	internal class Pmd_ObjectDef
	{
		[JsonPropertyOrder(-100)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public PmdTargetTypeID ObjectID { get; set; }
		
		[JsonPropertyOrder(-99)]
		public byte SlotOrID_Field01 { get; set; }
		[JsonPropertyOrder(-98)]
		public short Field02 { get; set; }
		[JsonPropertyOrder(-97)]
		public short Field04 { get; set; }
		[JsonPropertyOrder(-96)]
		public sbyte Field06 { get; set; }
		[JsonPropertyOrder(-95)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();
		[JsonPropertyOrder(-94)]
		public uint Field10 { get; set; }

		public void ReadObject(BinaryReader reader)
		{
			ObjectID = (PmdTargetTypeID)reader.ReadByte();
			SlotOrID_Field01 = reader.ReadByte();
			Field02 = reader.ReadInt16();
			Field04 = reader.ReadInt16();
			Field06 = reader.ReadSByte();
			Data = reader.ReadBytes(9);
			Field10 = reader.ReadUInt32();
		}

		public void WriteObject(BinaryWriter writer)
		{ 
			writer.Write((byte)ObjectID);
			writer.Write(SlotOrID_Field01);
			writer.Write(Field02);
			writer.Write(Field04);
			writer.Write(Field06);
			writer.Write(Data);
			writer.Write(Field10);
		}
	}
}
