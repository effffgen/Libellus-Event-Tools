using LibellusLibrary.Event.Types.Object;
using LibellusLibrary.JSON;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types
{
	internal class PmdData_ObjectTable : PmdDataType, ITypeCreator
	{
		[JsonConverter(typeof(PmdObjectReader))]
		public List<PmdObject> Objects { get; set; }

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
				PmdObject currentObject = new PmdObject();
				currentObject.ReadObject(reader);
				Objects.Add(currentObject);
			}

			reader.BaseStream.Position = OriginalPos;
			return this;
		}

		internal override void SaveData(PmdBuilder builder, BinaryWriter writer)
		{
			foreach(PmdObject obj in Objects)
			{
				obj.WriteObject(writer);
			}
		}
		internal override int GetCount() => Objects.Count;
		internal override int GetSize() => 0x14;
	}
}
