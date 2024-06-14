using LibellusLibrary.JSON;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types
{
	internal class PmdData_BezierTable : PmdDataType, ITypeCreator
	{
		[JsonInclude]
		public List<Pmd_BezierDef> Beziers { get; set; }
		
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
				Pmd_BezierDef currentBezier = new Pmd_BezierDef();
				currentBezier.ReadBezier(reader);
				Beziers.Add(currentBezier);
			}

			reader.BaseStream.Position = OriginalPos;
			return this;
		}
		
		internal override void SaveData(PmdBuilder builder, BinaryWriter writer)
		{
			foreach(Pmd_BezierDef bez in Beziers)
			{
				bez.WriteBezier(writer);
			}
		}
		internal override int GetCount() => Beziers.Count;
		internal override int GetSize() => 0x130;
	}
	
	internal class Pmd_BezierDef
	{
		[JsonPropertyOrder(-100)]
		public uint Field00 { get; set; }
		[JsonPropertyOrder(-99)]
		public float[] Data { get; set; } = new float[75];
		
		public void ReadBezier(BinaryReader reader)
		{
			Field00 = reader.ReadUInt32();
			for (int i = 0; i < 75; i++)
			{
				Data[i] = reader.ReadSingle();
			}
		}

		public void WriteBezier(BinaryWriter writer)
		{ 
			writer.Write(Field00);
			for (int i = 0; i < 75; i++)
			{
				writer.Write(Data[i]);
			}
		}
	}
}
