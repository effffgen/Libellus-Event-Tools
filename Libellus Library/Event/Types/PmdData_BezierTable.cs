using LibellusLibrary.Event.Types.Bezier;
using LibellusLibrary.JSON;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types
{
	internal class PmdData_BezierTable : PmdDataType, ITypeCreator
	{
		[JsonConverter(typeof(PmdBezierReader))]
		public List<PmdBezier> Beziers { get; set; }
		
		public PmdDataType? CreateType(uint version)
		{
			return version switch
			{
				12 => new PmdData_BezierTable(),
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

			Beziers = new();
			for (int i = 0; i < count; i++)
			{
				PmdBezier currentBezier = new PmdBezier();
				currentBezier.ReadBezier(reader);
				Beziers.Add(currentBezier);
			}

			reader.BaseStream.Position = OriginalPos;
			return this;
		}
		
		internal override void SaveData(PmdBuilder builder, BinaryWriter writer)
		{
			foreach(PmdBezier bez in Beziers)
			{
				bez.WriteBezier(writer);
			}
		}
		internal override int GetCount() => Beziers.Count;
		internal override int GetSize() => 0x130;
	}
}
