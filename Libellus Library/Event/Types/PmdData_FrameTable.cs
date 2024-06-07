using LibellusLibrary.Event.Types.Frame;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types
{
	internal class PmdData_FrameTable : PmdDataType, ITypeCreator
	{
		[JsonConverter(typeof(PmdFrameReader))]
		public List<PmdTargetType> Frames { get; set; }

		public PmdDataType? CreateType(uint version)
		{
			return version switch
			{
				12 => new PmdData_FrameTable(),
				_ => new PmdData_RawData()
			};
		}

		public PmdDataType? ReadType(BinaryReader reader, uint version, List<PmdTypeID> typeIDs, PmdTypeFactory typeFactory)
		{
			long OriginalPos = reader.BaseStream.Position;

			reader.BaseStream.Position = OriginalPos + 0x8;
			uint count = reader.ReadUInt32();

			reader.BaseStream.Position = OriginalPos + 0xC;
			reader.BaseStream.Position = (long)reader.ReadUInt32();

			PmdFrameFactory factory = new();
			Frames = factory.ReadDataTypes(reader, count);

			reader.BaseStream.Position = OriginalPos;
			return this;
		}

		internal override void SaveData(PmdBuilder builder, BinaryWriter writer)
		{
			foreach(PmdTargetType frame in Frames)
			{
				frame.WriteFrame(writer);
			}
		}
		internal override int GetCount() => Frames.Count;
		internal override int GetSize() => 0x3C; // Data size doesnt matter for certain types

	}


}
