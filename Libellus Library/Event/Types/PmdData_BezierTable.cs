using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types
{
	internal class PmdData_BezierTable : PmdDataType, ITypeCreator
	{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		[JsonInclude]
		public List<Pmd_BezierDef> Beziers { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

		public PmdDataType? CreateType(uint version)
		{
			return version switch
			{
				10 or 11 or 12 => new PmdData_BezierTable(),
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
		public byte Field00 { get; set; }
		
		[JsonPropertyOrder(-99)]
		public byte BezierMax { get; set; }
		
		[JsonPropertyOrder(-98)]
		public ushort Field02 { get; set; }

		// TODO: Can the size be fully determined at run-time without missing any data?
		[JsonPropertyOrder(-97)]
		public Pmd_Vector3[][] Data { get; set; } = new Pmd_Vector3[8][];
		
		public void ReadBezier(BinaryReader reader)
		{
			Field00 = reader.ReadByte();
			BezierMax = reader.ReadByte();
			Field02 = reader.ReadUInt16();
			for (int i = 0; i < Data.Length; i++)
			{
				if (i == 0)
				{
					Data[i] = new Pmd_Vector3[4];
				}
				else
				{
					Data[i] = new Pmd_Vector3[3];
				}
				for (int v = 0; v < Data[i].Length; v++)
				{
					Data[i][v] = new Pmd_Vector3();
					Data[i][v].X = reader.ReadSingle();
					Data[i][v].Y = reader.ReadSingle();
					Data[i][v].Z = reader.ReadSingle();
				}
			}
		}

		public void WriteBezier(BinaryWriter writer)
		{ 
			writer.Write(Field00);
			writer.Write(BezierMax);
			writer.Write(Field02);
			foreach (Pmd_Vector3[] curBez in Data)
			{
				foreach (Pmd_Vector3 curCoord in curBez)
				{
					writer.Write(curCoord.X);
					writer.Write(curCoord.Y);
					writer.Write(curCoord.Z);
				}
			}
		}
	}

	internal class Pmd_Vector3
	{
		public float X { get; set; }
		public float Y { get; set; }
		public float Z { get; set; }
	}

	/*
		BezierTable is ordered as follows:
			All points are type Vector3 (X, Y, Z floats)
			PIVOT = start of curve
			EASE IN = curve will "ease in" towards this position
			EASE OUT = curve will "ease out" towards this position
			PIVOT_E = end of curve
		When BezierMax > 1, (EASE IN, EASE OUT, PIVOT_E) points are appended to current bezier curve.
	*/
}
